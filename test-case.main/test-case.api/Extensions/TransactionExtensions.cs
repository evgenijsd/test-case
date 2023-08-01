using System.Globalization;
using test_case.api.Enums;
using test_case.api.Models.DTO;
using test_case.api.Models.Entities;
using test_case.api.Models.Transaction;

namespace test_case.api.Extensions
{
    public static class TransactionExtensions
    {
        public static Transaction ToTransaction(this TransactionCsvModel transaction)
        {
            var amount = transaction.Amount?.Replace("$", "") ?? string.Empty;

            return new Transaction
            {
                Id = transaction.TransactionId,
                Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), transaction.Status ?? string.Empty, true),
                Type = (TransactionType)Enum.Parse(typeof(TransactionType), transaction.Type ?? string.Empty, true),
                ClientName = transaction.ClientName,
                Amount = decimal.Parse(amount.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
            };
        }

        public static TransactionDTO ToTransactionDTO(this Transaction transaction)
        {
            return new TransactionDTO
            {
                Id = transaction.Id,
                Status = $"{transaction.Status}",
                Type = $"{transaction.Type}",
                ClientName = transaction.ClientName,
                Amount = $"${transaction.Amount}".Replace(',', '.')
            };
        }

        public static void UpdateTransaction(this Transaction updateTransaction, Transaction transaction)
        {
            updateTransaction.Id = transaction.Id;
            updateTransaction.Status = transaction.Status;
            updateTransaction.Type = transaction.Type;
            updateTransaction.ClientName = transaction.ClientName;
            updateTransaction.Amount = transaction.Amount;
        }
    }
}
