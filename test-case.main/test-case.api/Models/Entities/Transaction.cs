using test_case.api.Enums;

namespace test_case.api.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
        public string? ClientName { get; set; }
        public decimal Amount { get; set; }
    }
}
