using CsvHelper.Configuration.Attributes;

namespace test_case.api.Models.Transaction
{
    public class ExportCsvModel
    {
        public string? TransactionId { get; set; }
        public string? Name { get; set; }
        public string? Amount { get; set; }
        public DateTime Created { get; set; }
    }
}
