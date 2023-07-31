namespace test_case.api.Models.Authentication
{
    /// <summary>
    /// Data for user authentication.
    /// </summary>
    public class AuthenticationData
    {
        /// <summary>
        /// Access token for the authenticated user.
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// Refresh token for the authenticated user.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
