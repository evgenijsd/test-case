using test_case.api.Models.DTO;
using test_case.api.Models.Transaction;

namespace test_case.api.Interfaces
{
    public interface ITransactionService
    {
        Task ImportTransactionsAsync(IFormFile file);
        Task<byte[]> ExportTransactionsToCsvAsync();
        Task<List<TransactionDTO>> GetTransactionsByUserTimeZone(DateTime dateFrom, DateTime dateTo);
        Task<List<TransactionDTO>> GetTransactionsByClientsTimeZones(DateTime dateFrom, DateTime dateTo);
    }
}
