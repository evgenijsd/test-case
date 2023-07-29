using FluentValidation;
using test_case.api.Models.DTO;

namespace test_case.api.Validators
{
    public class RefreshTokenDTOValidator : AbstractValidator<RefreshTokenDTO>
    {
        public RefreshTokenDTOValidator()
        {
            RuleFor(t => t.Token)
                .NotEmpty()
                    .WithMessage("Token is mandatory.");
        }
    }
}
