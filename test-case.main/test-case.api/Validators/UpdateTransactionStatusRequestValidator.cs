using FluentValidation;
using test_case.api.Models.DTO;
using test_case.api.Models.Transaction;

namespace test_case.api.Validators
{
    public class UpdateTransactionStatusRequestValidator : AbstractValidator<UpdateTransactionStatusRequest>
    {
        public UpdateTransactionStatusRequestValidator()
        {
            RuleFor(u => u.NewStatus)
                .NotEmpty()
                    .WithMessage("Status is required.");

            RuleFor(u => u.TransactionId)
                .GreaterThan(0)
                    .WithMessage("TransactionId must be greater than 0.");
        }
    }
}
