using FluentValidation;
using test_case.api.Models.DTO;

namespace test_case.api.Validators
{
    public class AccessTokenDTOValidator : AbstractValidator<AccessTokenDTO>
    {
        public AccessTokenDTOValidator()
        {
            RuleFor(t => t.Token)
                .NotEmpty()
                    .WithMessage("Token is mandatory.");
        }
    }
}
