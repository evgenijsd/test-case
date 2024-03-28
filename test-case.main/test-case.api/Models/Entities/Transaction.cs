namespace test_case.api.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string? TransactionId { get; set; }
        public DateTime Created { get; set; }
        public int Offset { get; set; }
        public string? TimeZone { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
