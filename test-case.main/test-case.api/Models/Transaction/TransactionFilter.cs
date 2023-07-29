using System.Transactions;

namespace test_case.api.Models.Transaction
{
    public class TransactionFilter
    {
        public List<string>? TransactionTypes { get; set; }
        public TransactionStatus Status { get; set; }
        public string? ClientName { get; set; }
    }
}
