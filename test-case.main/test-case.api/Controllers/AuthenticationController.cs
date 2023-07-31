using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using test_case.api.Interfaces;
using test_case.api.Models.Authentication;
using test_case.api.Models.DTO;

namespace test_case.api.Controllers
{
    /// <summary>
    /// Controller for handling user authentication.
    /// </summary>
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

        /// <summary>
        /// User login into the system.
        /// </summary>
        /// <param name="userLoginDTO">Data for user login</param>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "User login into the system")]
        [SwaggerResponse(200, "Access and refresh tokens successfully generated", typeof(AuthenticationData))]
        public async Task<ActionResult<AuthenticationData>> Login(UserLoginDTO userLoginDTO)
        {
            return Ok(await _authenticationService.GenerateTokensAsync(userLoginDTO));
        }

        /// <summary>
        /// Refresh the access token using the refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token</param>
        [HttpPost("refresh")]
        [SwaggerOperation(Summary = "Refresh the access token using the refresh token")]
        [SwaggerResponse(200, "Access token successfully refreshed", typeof(AuthenticationData))]
        public async Task<ActionResult<AuthenticationData>> RefreshAccessToken(RefreshTokenDTO refreshToken)
        {
            return Ok(await _authenticationService.RefreshAccessTokenAsync(refreshToken));
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userRegisterDTO">Data of the new user</param>
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user")]
        [SwaggerResponse(200, "Access and refresh tokens successfully generated", typeof(AuthenticationData))]
        public async Task<ActionResult<AuthenticationData>> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            return Ok(await _authenticationService.RegisterAsync(userRegisterDTO));
        }

        /// <summary>
        /// Validate the access token for its validity.
        /// </summary>
        /// <param name="token">The access token</param>
        [HttpPost("validate")]
        [SwaggerOperation(Summary = "Validate the access token for its validity")]
        [SwaggerResponse(200, "Access token is valid")]
        [SwaggerResponse(401, "Access token is invalid")]
        public async Task<IActionResult> ValidationAccessToken(AccessTokenDTO token)
        {
            return Ok(await _authenticationService.ValidateAccessTokenAsync(token));
        }
    }
}
