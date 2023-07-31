using Swashbuckle.AspNetCore.Annotations;

namespace test_case.api.Models.DTO
{
    /// <summary>
    /// Data transfer object for refresh tokens.
    /// </summary>
    public class RefreshTokenDTO
    {
        /// <summary>
        /// Refresh token.
        /// </summary>
        [SwaggerSchema(Description = "Refresh token")]
        public string? Token { get; set; }
    }
}
