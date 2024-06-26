﻿using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Data;
using System.Globalization;
using System.Text;
using test_case.api.Context;
using test_case.api.Context.Migrations;
using test_case.api.Converter;
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
        public TransactionService(TestCaseContext context, IDbConnection dbConnection) : base(context, dbConnection)
        {
        }

        public async Task<byte[]> ExportTransactionsToCsvAsync()
        {
            string sqlQuery = @"
                SELECT t.TransactionId, c.Name, t.Amount, DATEADD(second, Offset, t.Created) AS Created
                FROM Transactions t
                INNER JOIN Clients c ON t.ClientId = c.Id
                ORDER BY t.Created DESC
            ";

            var transactions = await _dbConnection.QueryAsync<ExportCsvModel>(sqlQuery);

            var csvData = new StringBuilder();
            var columns = new List<string>() { "TransactionId", "Name", "Amount", "Created" };
            csvData.AppendLine(string.Join(",", columns));

            foreach (var transaction in transactions)
            {
                var rowValues = columns.Select(column =>
                {
                    return column switch
                    {
                        "TransactionId" => $"{transaction.TransactionId}",
                        "Name" => $"{transaction.Name}",
                        "Amount" => $"${transaction.Amount}".Replace(',', '.'),
                        "Created" => $"{transaction.Created.ToString("dd.MM.yyyy HH:mm")}",
                        _ => string.Empty,
                    };
                });
                csvData.AppendLine(string.Join(",", rowValues));
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvData.ToString());

            return csvBytes;
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
                var client = await _context.Clients.FirstOrDefaultAsync(x => x.Email == record.Email);

                if (client == null) 
                {
                    client = new Client { Name = record.Name, Email = record.Email };
                    await _context.Clients.AddAsync(client); 
                }
                records.Add(record.ToTransaction(client.Id));
            }

            await _context.SaveChangesAsync();
            var transactionIds = records.Select(r => r.TransactionId).ToList(); 
            var existingTransactions = await _context.Transactions
                .Where(t => transactionIds.Contains(t.TransactionId))
                .ToListAsync();

            await _context.Database.OpenConnectionAsync();

            foreach (var record in records)
            {
                var existingTransaction = existingTransactions.FirstOrDefault(t => t.TransactionId == record.TransactionId);

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
        }

        public async Task<List<TransactionDTO>> GetTransactionsByUserTimeZone(DateTime dateFrom, DateTime dateTo)
        {
            var timeZoneConverter = new TimeZoneConverter();
            var zone = DateTimeZoneProviders.Tzdb.GetSystemDefault().Id;
            List<DateOffset> dateOffsets = new List<DateOffset>();

            for (DateTime date = dateFrom.Date; date <= dateTo.Date; date = date.AddDays(1))
            {
                int offset = timeZoneConverter.GetOffsetByDateInSeconds(date, zone);
                dateOffsets.Add(new DateOffset { Date = date, Offset = offset });
            }

            var sqlQuery = @"
                DECLARE @DateOffsets TABLE (Date DATE, Offset INT);" +
                string.Join(" ", dateOffsets.Select(dateOffset =>
                    $"INSERT INTO @DateOffsets (Date, Offset) VALUES ('{dateOffset.Date:yyyy-MM-dd HH:mm:ss.fff}', {dateOffset.Offset});")) +
                @"SELECT t.TransactionId, 
                    DATEADD(second, Offset, t.Created) AS Created, 
                    t.Amount, c.Name, c.Email
                FROM Transactions t
                JOIN Clients c ON t.ClientId = c.Id
                WHERE DATEADD(second, t.Offset, t.Created) >= @DateFrom 
                    AND DATEADD(second, t.Offset, t.Created) <= @DateTo
                    AND EXISTS (
                        SELECT 1 
                        FROM @DateOffsets d
                        WHERE CAST(d.Date AS DATE) = CAST(DATEADD(second, t.Offset, t.Created) AS DATE) AND d.Offset = t.Offset
                    )
                ORDER BY t.Created DESC
            ";

            var dateOffsetValues = dateOffsets.Select(d => new { d.Date.Date, d.Offset });
            var transactions = await _dbConnection.QueryAsync<TransactionDTO>(sqlQuery, 
                    new { DateFrom = dateFrom, DateTo = dateTo });

            return transactions.ToList();
        }

        public async Task<List<TransactionDTO>> GetTransactionsByClientsTimeZones(DateTime dateFrom, DateTime dateTo)
        {
            string sqlQuery = @"
                SELECT t.TransactionId, 
                    DATEADD(second, Offset, t.Created) AS Created, 
                    t.Amount, c.Name, c.Email
                FROM Transactions t
                JOIN Clients c ON t.ClientId = c.Id
                WHERE DATEADD(second, [Offset], [Created]) >= @DateFrom AND DATEADD(second, [Offset], [Created]) <= @DateTo
                ORDER BY t.Created DESC
            ";

            var transactions = await _dbConnection.QueryAsync<TransactionDTO>(sqlQuery, new { DateFrom = dateFrom, DateTo = dateTo });

            return transactions.ToList();
        }

        public struct DateOffset
        {
            public DateTime Date;
            public int Offset;
        }
    }
}
