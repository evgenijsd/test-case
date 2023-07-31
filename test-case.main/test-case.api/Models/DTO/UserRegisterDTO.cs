namespace test_case.api.Models.DTO
{
    /// <summary>
    /// Data transfer object for user registration information.
    /// </summary>
    public class UserRegisterDTO : UserDTO
    {
        /// <summary>
        /// User's password.
        /// </summary>
        public string? Password { get; set; }
    }
}
