using FluentValidation;
using test_case.api.Models.DTO;
using test_case.api.Models.Transaction;

namespace test_case.api.Validators
{
    public class UpdateTransactionStatusRequestValidator : AbstractValidator<UpdateTransactionStatusRequest>
    {
        public UpdateTransactionStatusRequestValidator()
        {
            RuleFor(u => u.TransactionId)
                .NotEmpty()
                    .WithMessage("TransactionId must be not empty.");
        }
    }
}
