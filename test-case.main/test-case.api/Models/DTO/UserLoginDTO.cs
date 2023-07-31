using Swashbuckle.AspNetCore.Annotations;

namespace test_case.api.Models.DTO
{
    /// <summary>
    /// Data transfer object for user login information.
    /// </summary>
    public class UserLoginDTO
    {
        /// <summary>
        /// User's email address.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// User's password.
        /// </summary>
        public string? Password { get; set; }
    }
}
