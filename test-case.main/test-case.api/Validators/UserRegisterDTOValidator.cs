using FluentValidation;
using test_case.api.Models.DTO;

namespace test_case.api.Validators
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty()
                    .WithMessage("UserName is mandatory.")
                .MaximumLength(50)
                    .WithMessage("UserName must not exceed 50 characters.")
                .Must(HasNoWhiteSpace)
                    .WithMessage("UserName must not contain white spaces.")
                .MinimumLength(3)
                    .WithMessage("UserName should be minimum 3 character.");

            RuleFor(u => u.Email)
                .NotEmpty()
                    .WithMessage("Email is required.")
                .EmailAddress()
                    .WithMessage("Invalid email format.")
                .MaximumLength(100)
                    .WithMessage("Email must not exceed 100 characters.");

            RuleFor(u => u.Password)
                .NotEmpty()
                    .WithMessage("Password is required.")
                .Length(4, 16)
                    .WithMessage("Password must be from 4 to 16 characters.");
        }

        private bool HasNoWhiteSpace(string name)
        {
            return !string.IsNullOrEmpty(name) && !name.Contains(" ");
        }
    }
}
