using test_case.api.Services;

namespace test_case.api.Models.Transaction
{
    public class TransactionCsvModel
    {
        public int TransactionId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? ClientName { get; set; }
        public string? Amount { get; set; }
    }
}
