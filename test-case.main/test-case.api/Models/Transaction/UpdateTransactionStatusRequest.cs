using test_case.api.Enums;

namespace test_case.api.Models.Transaction
{
    public class UpdateTransactionStatusRequest
    {
        public int TransactionId { get; set; }
        public string? NewStatus { get; set; }
    }
}
