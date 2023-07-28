using System.Security.Claims;
using test_case.api.Context;
using test_case.api.Interfaces;
using test_case.api.Models.Authentication;
using test_case.api.Services.Abstract;

namespace test_case.api.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        public AuthenticationService(TestCaseContext context) : base(context)
        {
        }

        public Task<AuthenticationResult> AuthenticateAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> RegisterAsync(string userName, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
