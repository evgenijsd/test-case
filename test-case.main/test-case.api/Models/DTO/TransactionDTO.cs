namespace test_case.api.Models.DTO
{
    /// <summary>
    /// Data transfer object for represents a transaction.
    /// </summary>
    public class TransactionDTO
    {
        public string? TransactionId { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }        
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
