using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using test_case.api.Enums;

namespace test_case.api.Models.Transaction
{
    public class TransactionQuery
    {
        public string? Type { get; set; }
        public string? Status { get; set; }
        [DefaultValue(new[] { "TransactionId", "Status", "Type", "ClientName", "Amount" })]
        public List<string> Columns { get; set; } = new List<string>();
    }
}
