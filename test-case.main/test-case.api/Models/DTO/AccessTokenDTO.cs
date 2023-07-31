using Swashbuckle.AspNetCore.Annotations;

namespace test_case.api.Models.DTO
{
    /// <summary>
    /// Data transfer object for access tokens.
    /// </summary>
    public class AccessTokenDTO
    {
        /// <summary>
        /// Access token.
        /// </summary>
        [SwaggerSchema(Description = "Access token")]
        public string? Token { get; set; }
    }
}
