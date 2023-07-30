using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using test_case.api.Enums;

namespace test_case.api.Models.Transaction
{
    public class TransactionFilter
    {
        public List<string> Types { get; set; } = new List<string>();
        public string? Status { get; set; }
        public string? ClientName { get; set; }
    }
}
