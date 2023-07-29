using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_case.api.Interfaces;
using test_case.api.Models.Authentication;
using test_case.api.Models.DTO;

namespace test_case.api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationData>> Login(UserLoginDTO userLoginDTO)
        {
            return Ok(await _authenticationService.GenerateTokensAsync(userLoginDTO));
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthenticationData>> RefreshAccessToken(RefreshTokenDTO refreshToken)
        {
            return Ok(await _authenticationService.RefreshAccessTokenAsync(refreshToken));
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationData>> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            return Ok(await _authenticationService.RegisterAsync(userRegisterDTO));
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidationAccessToken(AccessTokenDTO token)
        {
            return Ok(await _authenticationService.ValidateAccessTokenAsync(token));
        }
    }
}
