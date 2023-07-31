using Swashbuckle.AspNetCore.Annotations;

namespace test_case.api.Models.DTO
{
    /// <summary>
    /// Data transfer object for user information.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// User's email address.
        /// </summary>
        [SwaggerSchema(Description = "User's email address")]
        public string? Email { get; set; }
        /// <summary>
        /// User's name.
        /// </summary>
        [SwaggerSchema(Description = "User's name")]
        public string? UserName { get; set; }
    }
}
