using test_case.api.Enums;

namespace test_case.api.Models.Entities
{
    /// <summary>
    /// Represents a transaction.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// The unique identifier of the transaction.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The status of the transaction (Pending, Completed, Canceled).
        /// </summary>
        public TransactionStatus Status { get; set; }
        /// <summary>
        /// The type of the transaction (Refill, Withdrawal).
        /// </summary>
        public TransactionType Type { get; set; }
        /// <summary>
        /// The name of the client associated with the transaction.
        /// </summary>
        public string? ClientName { get; set; }
        /// <summary>
        /// The amount of the transaction.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
