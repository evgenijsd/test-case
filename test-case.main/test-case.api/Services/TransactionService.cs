using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;
using System.Reflection;
using System.Transactions;
using test_case.api.Context;
using test_case.api.Exceptions;
using test_case.api.Interfaces;
using test_case.api.Models.Transaction;
using test_case.api.Services.Abstract;

namespace test_case.api.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(TestCaseContext context) : base(context)
        {
        }

        public Task<byte[]> ExportTransactionsToCsvAsync(List<Transaction> transactions)
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> GetFilteredTransactionsAsync(TransactionFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task ImportTransactionsAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                throw new NoFileException();
            }

            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };
            using var csv = new CsvReader(reader, config);
            var records = new List<TransactionCsvModel>();
            await foreach (var record in csv.GetRecordsAsync<TransactionCsvModel>())
            {
                records.Add(record);
            }
        }

        public Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus newStatus)
        {
            throw new NotImplementedException();
        }
    }
}
