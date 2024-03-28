using CsvHelper.Configuration.Attributes;
using test_case.api.Services;

namespace test_case.api.Models.Transaction
{
    public class TransactionCsvModel
    {
        [Name("transaction_id")]
        public string? TransactionId { get; set; }

        [Name("name")]
        public string? Name { get; set; }

        [Name("email")]
        public string? Email { get; set; }

        [Name("amount")]
        public string? Amount { get; set; }

        [Name("transaction_date")]
        public string? TransactionDate { get; set; }

        [Name("client_location")]
        public string? ClientLocation { get; set; }
    }
}
