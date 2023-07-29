using System.Transactions;
using test_case.api.Models.Transaction;

namespace test_case.api.Interfaces
{
    public interface ITransactionService
    {
        Task ImportTransactionsAsync(IFormFile file);
        Task<List<Transaction>> GetFilteredTransactionsAsync(TransactionFilter filter);
        Task<byte[]> ExportTransactionsToCsvAsync(List<Transaction> transactions);
        Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus newStatus);
    }
}
