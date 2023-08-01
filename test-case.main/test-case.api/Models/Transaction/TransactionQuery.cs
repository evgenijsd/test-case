using System.ComponentModel;

namespace test_case.api.Models.Transaction
{
    /// <summary>
    /// Represents a query for exporting transactions to CSV with optional filtering and selected columns.
    /// </summary>
    public class TransactionQuery
    {
        /// <summary>
        /// Transaction type to filter by (Refill, Withdrawal).
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// Transaction status to filter by (Pending, Completed, Canceled).
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// List of columns to include in the exported CSV file. Default columns are: TransactionId, Status, Type, ClientName, Amount.
        /// </summary>
        [DefaultValue(new[] { "TransactionId", "Status", "Type", "ClientName", "Amount" })]
        public List<string> Columns { get; set; } = new List<string>();
    }
}
