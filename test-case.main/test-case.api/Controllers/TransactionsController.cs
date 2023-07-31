using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Swashbuckle.AspNetCore.Annotations;
using test_case.api.Enums;
using test_case.api.Interfaces;
using test_case.api.Models.Entities;
using test_case.api.Models.Transaction;

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
        /// Export transactions to a CSV file using filters.
        /// </summary>
        /// <param name="query">Object with filtering parameters</param>
        [HttpGet("export")]
        [SwaggerOperation(Summary = "Export transactions to a CSV file using filters")]
        [SwaggerResponse(200, "CSV file with transactions", typeof(byte[]))]
        public async Task<IActionResult> ExportTransactionsToCsv([FromQuery] TransactionQuery query)
        {
            var csvBytes = await _transactionService.ExportTransactionsToCsvAsync(query);

            return File(csvBytes, "text/csv", "transactions.csv");
        }

        /// <summary>
        /// Get a list of transactions with filtering.
        /// </summary>
        /// <param name="filter">Object with filtering parameters</param>
        [HttpGet("filtered")]
        [SwaggerOperation(Summary = "Get a list of transactions with filtering")]
        [SwaggerResponse(200, "List of transactions", typeof(List<Transaction>))]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilter filter)
        {

            return Ok(await _transactionService.GetFilteredTransactionsAsync(filter));
        }

        /// <summary>
        /// Update the status of a transaction.
        /// </summary>
        /// <param name="request">Object with data for updating the status</param>
        [HttpPost("update-status")]
        [SwaggerOperation(Summary = "Update the status of a transaction")]
        [SwaggerResponse(200, "Transaction status updated")]
        [SwaggerResponse(400, "Bad request or invalid transaction ID")]
        public async Task<IActionResult> UpdateTransactionStatus([FromBody] UpdateTransactionStatusRequest request)
        {
            await _transactionService.UpdateTransactionStatusAsync(request.TransactionId, request.NewStatus);
            return Ok("Transaction status updated successfully.");
        }
    }
}
