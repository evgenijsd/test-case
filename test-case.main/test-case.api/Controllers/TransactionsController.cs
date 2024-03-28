using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.SqlServer.Server;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using test_case.api.Interfaces;
using test_case.api.Models.DTO;

namespace test_case.api.Controllers
{
    /// <summary>
    /// API endpoints for managing transactions.
    /// </summary>
    [ApiController]
    [Route("api/transactions")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Import transactions from an Excel file.
        /// </summary>
        /// <param name="file">Excel file with transactions</param>
        [HttpPost("import")]
        [SwaggerOperation(Summary = "Import transactions from an Excel file")]
        public async Task<IActionResult> ImportTransactions(IFormFile file)
        {
            await _transactionService.ImportTransactionsAsync(file);
            return Ok("Transactions imported successfully.");
        }

        /// <summary>
        /// Export transactions to a CSV file.
        /// </summary>
        [HttpGet("export")]
        [SwaggerOperation(Summary = "Export transactions to a CSV file")]
        [SwaggerResponse(200, "CSV file with transactions", typeof(byte[]))]
        public async Task<IActionResult> ExportTransactionsToCsv()
        {
            var csvBytes = await _transactionService.ExportTransactionsToCsvAsync();
            return File(csvBytes, "text/csv", "transactions.csv");
        }

        [HttpGet("user_transactions_year")]
        [SwaggerOperation(Summary = "Get a list of transactions")]
        [SwaggerResponse(200, "List of transactions", typeof(List<TransactionDTO>))]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsYear([BindRequired] int year)
        {
            var userDateFrom = new DateTime(year, 1, 1, 0, 0, 0);
            var userDateTo = new DateTime(year, 12, 31, 23, 59, 59);
            return Ok(await _transactionService.GetTransactionsByUserTimeZone(userDateFrom, userDateTo));
        }

        [HttpGet("clients_transactions_year")]
        [SwaggerOperation(Summary = "Get a list of transactions")]
        [SwaggerResponse(200, "List of transactions", typeof(List<TransactionDTO>))]
        public async Task<ActionResult<List<TransactionDTO>>> GetClientsTransactionsYear([BindRequired] int year)
        {
            var userDateFrom = new DateTime(year, 1, 1, 0, 0, 0);
            var userDateTo = new DateTime(year, 12, 31, 23, 59, 59);
            return Ok(await _transactionService.GetTransactionsByClientsTimeZones(userDateFrom, userDateTo));
        }

        [HttpGet("user_transactions_month")]
        [SwaggerOperation(Summary = "Get a list of transactions")]
        [SwaggerResponse(200, "List of transactions", typeof(List<TransactionDTO>))]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsMonth([BindRequired] int year, [BindRequired] int month)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var userDateFrom = new DateTime(year, month, 1, 0, 0, 0);
            var userDateTo = new DateTime(year, month, daysInMonth, 23, 59, 59);
            return Ok(await _transactionService.GetTransactionsByUserTimeZone(userDateFrom, userDateTo));
        }

        [HttpGet("clients_transactions_month")]
        [SwaggerOperation(Summary = "Get a list of transactions")]
        [SwaggerResponse(200, "List of transactions", typeof(List<TransactionDTO>))]
        public async Task<ActionResult<List<TransactionDTO>>> GetClientsTransactionsMonth([BindRequired] int year, [BindRequired] int month)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var userDateFrom = new DateTime(year, month, 1, 0, 0, 0);
            var userDateTo = new DateTime(year, month, daysInMonth, 23, 59, 59);
            return Ok(await _transactionService.GetTransactionsByClientsTimeZones(userDateFrom, userDateTo));
        }

        [HttpGet("transactions/between_dates")]
        [SwaggerOperation(Summary = "Get a list of transactions")]
        [SwaggerResponse(200, "List of transactions", typeof(List<TransactionDTO>))]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsBetweenDates(
            [FromQuery(Name = "Date from: dd.MM.yyyy")] string dateFrom,
            [FromQuery(Name = "Date to: dd.MM.yyyy")] string dateTo)
        {
            var userDateFrom = DateTime.ParseExact(dateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture).Date;
            var userDateTo = DateTime.ParseExact(dateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture).Date.AddDays(1).AddTicks(-1);

            return Ok(await _transactionService.GetTransactionsByUserTimeZone(userDateFrom, userDateTo));
        }

        [HttpGet("transactions/between_dates_clients")]
        [SwaggerOperation(Summary = "Get a list of transactions")]
        [SwaggerResponse(200, "List of transactions", typeof(List<TransactionDTO>))]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsBetweenDatesClients(
            [FromQuery(Name = "Date from: dd.MM.yyyy")] string dateFrom,
            [FromQuery(Name = "Date to: dd.MM.yyyy")] string dateTo)
        {
            var clientsDateFrom = DateTime.ParseExact(dateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture).Date;
            var clientsDateTo = DateTime.ParseExact(dateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture).Date.AddDays(1).AddTicks(-1);

            return Ok(await _transactionService.GetTransactionsByClientsTimeZones(clientsDateFrom, clientsDateTo));
        }
    }
}
