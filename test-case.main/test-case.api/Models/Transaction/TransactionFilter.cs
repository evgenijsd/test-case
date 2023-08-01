using System.ComponentModel;

namespace test_case.api.Models.Transaction
{
    /// <summary>
    /// Represents a filter for transactions.
    /// </summary>
    public class TransactionFilter
    {
        /// <summary>
        /// List of transaction types to filter by (Refill, Withdrawal).
        /// </summary>
        [DefaultValue(new[] { "Refill", "Withdrawal" })]
        public List<string> Types { get; set; } = new List<string>();
        /// <summary>
        /// Status of the transactions to filter by (Pending, Completed, Canceled).
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// Client name to filter by.
        /// </summary>
        public string? ClientName { get; set; }
    }
}
