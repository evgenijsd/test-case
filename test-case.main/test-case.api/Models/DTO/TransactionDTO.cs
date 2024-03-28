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
        public string? TransactionId { get; set; }
        /// <summary>
        /// Transaction creation date.
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// The amount of the transaction.
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// The name of the client associated with the transaction.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// The email of the client associated with the transaction.
        /// </summary>
        public string? Email { get; set; }
    }
}
