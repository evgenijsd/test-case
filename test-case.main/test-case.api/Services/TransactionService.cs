using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using test_case.api.Context;
using test_case.api.Enums;
using test_case.api.Exceptions;
using test_case.api.Extensions;
using test_case.api.Interfaces;
using test_case.api.Models.DTO;
using test_case.api.Models.Entities;
using test_case.api.Models.Transaction;
using test_case.api.Services.Abstract;

namespace test_case.api.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(TestCaseContext context) : base(context)
        {
        }

        public async Task<byte[]> ExportTransactionsToCsvAsync(TransactionQuery query)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.AppendLine("SELECT Id, Status, Type, ClientName, Amount " +
                       "FROM Transactions " +
                       "WHERE 1=1 ");

            if (!string.IsNullOrEmpty(query.Type))
            {
                sqlQuery.AppendLine($"AND Type = {(int)Enum.Parse(typeof(TransactionType), query.Type, true)} ");
            }

            if (!string.IsNullOrEmpty(query.Status))
            {
                sqlQuery.AppendLine($"AND Status = {(int)Enum.Parse(typeof(TransactionStatus), query.Status, true)}");
            }

            var transactions = await _context.Transactions
                .FromSqlRaw(sqlQuery.ToString())
                .ToListAsync();

            var csvData = new StringBuilder();
            csvData.AppendLine(string.Join(",", query.Columns));

            foreach (var transaction in transactions)
            {
                var rowValues = query.Columns.Select(column =>
                {
                    return column switch
                    {
                        "TransactionId" => $"{transaction.Id}",
                        "Status" => $"{transaction.Status}",
                        "Type" => $"{transaction.Type}",
                        "ClientName" => transaction.ClientName,
                        "Amount" => $"${transaction.Amount}".Replace(',', '.'),
                        _ => string.Empty,
                    };
                });
                csvData.AppendLine(string.Join(",", rowValues));
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvData.ToString());

            return csvBytes;
        }

        public async Task<List<TransactionDTO>> GetFilteredTransactionsAsync(TransactionFilter filter)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.AppendLine("SELECT * FROM Transactions WHERE 1=1");
            var parameters = new List<SqlParameter>();

            if (filter.Types.Count > 0)
            {
                var types = filter.Types.Select(t => (int)Enum.Parse(typeof(TransactionType), t, true)).ToList();
                sqlQuery.AppendLine("AND Type IN (" + string.Join(",", types.Select((_, i) => $"@type{i}")) + ")");
                parameters.AddRange(types.Select((t, i) => new SqlParameter($"@type{i}", t)));
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                var transactionStatus = (int)Enum.Parse<TransactionStatus>(filter.Status, true);
                sqlQuery.AppendLine("AND Status = @status");
                parameters.Add(new SqlParameter("@status", transactionStatus));
            }

            if (!string.IsNullOrEmpty(filter.ClientName))
            {
                sqlQuery.AppendLine("AND ClientName LIKE '%' + @clientName + '%'");
                parameters.Add(new SqlParameter("@clientName", filter.ClientName));
            }

            return await _context.Transactions
                .FromSqlRaw(sqlQuery.ToString(), parameters.ToArray())
                .Select(transaction => transaction.ToTransactionDTO()).ToListAsync();
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
            var records = new List<Transaction>();

            await foreach (var record in csv.GetRecordsAsync<TransactionCsvModel>())
            {
                records.Add(record.ToTransaction());
            }

            var transactionIds = records.Select(r => r.Id).ToList();
            var existingTransactions = await _context.Transactions
                .Where(t => transactionIds.Contains(t.Id))
                .ToListAsync();

            await _context.Database.OpenConnectionAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Transactions ON");

            foreach (var record in records)
            {
                var existingTransaction = existingTransactions.FirstOrDefault(t => t.Id == record.Id);

                if (existingTransaction != null)
                {
                    existingTransaction.UpdateTransaction(record);
                }
                else
                {
                    _context.Transactions.Add(record);
                }
            }

            await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Transactions OFF");
            await _context.Database.CloseConnectionAsync();
        }

        public async Task UpdateTransactionStatusAsync(int transactionId, string? newStatus)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);

            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction));
            }

            var status = (int)Enum.Parse(typeof(TransactionStatus), newStatus!, true);

            var sqlQuery = "UPDATE Transactions SET Status = @status WHERE Id = @id";
            var parameters = new[]
            {
                new SqlParameter("@status", status),
                new SqlParameter("@id", transactionId)
            };

            await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
        }
    }
}
