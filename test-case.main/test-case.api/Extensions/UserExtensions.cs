using test_case.api.Models.DTO;
using test_case.api.Models.Entities;

namespace test_case.api.Extensions
{
    public static class UserExtensions
    {
        public static User ToUser(this UserRegisterDTO user, string passwordHash)
        {
            return new User
            {
                Email = user.Email,
                UserName = user.UserName,
                PasswordHash = passwordHash,
                SecretKey = string.Empty,
            };
        }
    }
}
