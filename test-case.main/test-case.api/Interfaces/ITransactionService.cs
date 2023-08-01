using test_case.api.Models.DTO;
using test_case.api.Models.Transaction;

namespace test_case.api.Interfaces
{
    public interface ITransactionService
    {
        Task ImportTransactionsAsync(IFormFile file);
        Task<List<TransactionDTO>> GetFilteredTransactionsAsync(TransactionFilter filter);
        Task<byte[]> ExportTransactionsToCsvAsync(TransactionQuery query);
        Task UpdateTransactionStatusAsync(int transactionId, string? newStatus);
    }
}
