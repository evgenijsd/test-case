using test_case.api.Models.Authentication;

namespace test_case.api.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> RegisterAsync(string userName, string email, string password);
        Task<AuthenticationResult> AuthenticateAsync(string userName, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
    }
}
