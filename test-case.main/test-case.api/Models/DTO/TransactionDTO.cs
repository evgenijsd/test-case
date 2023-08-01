using test_case.api.Enums;

namespace test_case.api.Models.DTO
{
    /// <summary>
    /// Data transfer object for represents a transaction.
    /// </summary>
    public class TransactionDTO
    {
        /// <summary>
        /// The unique identifier of the transaction.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The status of the transaction (Pending, Completed, Canceled).
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// The type of the transaction (Refill, Withdrawal).
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// The name of the client associated with the transaction.
        /// </summary>
        public string? ClientName { get; set; }
        /// <summary>
        /// The amount of the transaction.
        /// </summary>
        public string? Amount { get; set; }
    }
}
