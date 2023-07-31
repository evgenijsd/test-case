using test_case.api.Enums;

namespace test_case.api.Models.Transaction
{
    /// <summary>
    /// Represents a request to update the status of a transaction.
    /// </summary>
    public class UpdateTransactionStatusRequest
    {
        /// <summary>
        /// The unique identifier of the transaction to update.
        /// </summary>
        public int TransactionId { get; set; }
        /// <summary>
        /// The new status to set for the transaction.
        /// </summary>
        public string? NewStatus { get; set; }
    }
}
