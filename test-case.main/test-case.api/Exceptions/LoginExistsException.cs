using test_case.api.Models.Entities;

namespace test_case.api.Exceptions
{
    public class LoginExistsException : Exception
    {
        public LoginExistsException(string name) : base($"{name} with this email already exists") { }
    }
}
