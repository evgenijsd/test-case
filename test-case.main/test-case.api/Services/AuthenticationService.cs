using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using test_case.api.Constants;
using test_case.api.Context;
using test_case.api.Exceptions;
using test_case.api.Extensions;
using test_case.api.Interfaces;
using test_case.api.Models.Authentication;
using test_case.api.Models.DTO;
using test_case.api.Models.Entities;
using test_case.api.Services.Abstract;

namespace test_case.api.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationService(TestCaseContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public async Task<AuthenticationData> GenerateTokensAsync(UserLoginDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email && u.PasswordHash == HashPassword(userDto.Password!));

            if (user == null)
            {
                throw new NotFoundException(nameof(User));
            }

            return new AuthenticationData
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = await GenerateRefreshTokenAsync(user),
            };
        }

        public async Task<AuthenticationData> RegisterAsync(UserRegisterDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user != null)
            {
                throw new LoginExistsException(nameof(User));
            }

            user = userDto.ToUser(HashPassword(userDto.Password!));
            _context.Add(user);
            await _context.SaveChangesAsync();

            return new AuthenticationData
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = await GenerateRefreshTokenAsync(user),
            };
        }

        public async Task<AuthenticationData> RefreshAccessTokenAsync(RefreshTokenDTO refreshToken)
        {
            var user = await ValidateRefreshTokenToUserIdAsync(refreshToken.Token ?? string.Empty);

            return new AuthenticationData
            {
                AccessToken = GenerateAccessToken(user!),
                RefreshToken = await GenerateRefreshTokenAsync(user!),
            };
        }

        public async Task<bool> ValidateAccessTokenAsync(AccessTokenDTO tokenDto)
        {
            var token = tokenDto.Token ?? string.Empty;
            var userId = GetUserIdFromClaimToken(token);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new InvalidTokenException("access");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration[ConfigurationConstants.SecretAccessKey]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            try
            {
                var principal = tokenHandler.ValidateToken(token, GetValidationParameters(securityKey), out var _);
            }
            catch (Exception)
            {
                throw new InvalidTokenException("access");
            }

            return true;
        }

        private string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[ConfigurationConstants.SecretAccessKey]!));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(10);

            if (double.TryParse(_configuration[ConfigurationConstants.AccessTokenExpiration], out double expiresValue))
            {
                expires = DateTime.UtcNow.AddMinutes(expiresValue);
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(GetTokenDescriptor(signingCredentials, user, expires));
        }

        private async Task<string> GenerateRefreshTokenAsync(User user)
        {
            var secretKey = Guid.NewGuid().ToString("N");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(10);

            if (double.TryParse(_configuration[ConfigurationConstants.RefreshTokenExpiration], out double expiresValue))
            {
                expires = DateTime.UtcNow.AddDays(expiresValue);
            }

            user.SecretKey = secretKey;
            await _context.SaveChangesAsync();

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(GetTokenDescriptor(signingCredentials, user, expires));
        }

        private JwtSecurityToken GetTokenDescriptor(SigningCredentials signingCredentials, User user, DateTime expires)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.UserName!),
                };

            return new JwtSecurityToken
            (
                issuer: _configuration[ConfigurationConstants.Issuer],
                audience: _configuration[ConfigurationConstants.Audience],
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials
            );
        }

        private async Task<User?> ValidateRefreshTokenToUserIdAsync(string token)
        {
            var userId = GetUserIdFromClaimToken(token);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new InvalidTokenException("refresh");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = user?.SecretKey ?? string.Empty;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            try
            {
                var principal = tokenHandler.ValidateToken(token, GetValidationParameters(securityKey), out var _);
            }
            catch (Exception)
            {
                throw new InvalidTokenException("refresh");
            }

            return user;

        }

        private int GetUserIdFromClaimToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValid = tokenHandler.CanReadToken(token);
            int userId = 0;

            if (tokenValid)
            {
                var claims = tokenHandler.ReadJwtToken(token).Claims;
                var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

                int.TryParse(userIdClaim, out userId);
            }

            return userId;
        }

        private TokenValidationParameters GetValidationParameters(SymmetricSecurityKey securityKey)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration[ConfigurationConstants.Issuer],
                ValidAudience = _configuration[ConfigurationConstants.Audience],
                IssuerSigningKey = securityKey
            }; 
        }

        private string HashPassword(string password)
        {
            var salt = Encoding.UTF8.GetBytes(_configuration[ConfigurationConstants.SecretAccessKey]!);
            return Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: password,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8
                    ));
        }

        private bool ValidatePassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
