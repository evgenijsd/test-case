using System.Transactions;

namespace test_case.api.Models.Transaction
{
    public class UpdateTransactionStatusRequest
    {
        public int TransactionId { get; set; }
        public TransactionStatus NewStatus { get; set; }
    }
}
