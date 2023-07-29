using test_case.api.Models.Authentication;
using test_case.api.Models.DTO;

namespace test_case.api.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationData> RegisterAsync(UserRegisterDTO user);
        Task<AuthenticationData> GenerateTokensAsync(UserLoginDTO userDto);
        Task<AuthenticationData> RefreshAccessTokenAsync(RefreshTokenDTO refreshToken);
        Task<bool> ValidateAccessTokenAsync(AccessTokenDTO token);
    }
}
