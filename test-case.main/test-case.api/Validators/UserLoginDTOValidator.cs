using FluentValidation;
using test_case.api.Models.DTO;

namespace test_case.api.Validators
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                    .WithMessage("Email is required.");

            RuleFor(u => u.Password)
                .NotEmpty()
                    .WithMessage("Password is required.");
        }
    }
}
