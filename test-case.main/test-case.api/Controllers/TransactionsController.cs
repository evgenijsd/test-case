using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Swashbuckle.AspNetCore.Annotations;
using test_case.api.Enums;
using test_case.api.Interfaces;
using test_case.api.Models.Transaction;

namespace test_case.api.Controllers
{
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

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions(IFormFile file)
        {
            await _transactionService.ImportTransactionsAsync(file);
            return Ok("Transactions imported successfully.");
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportTransactionsToCsv([FromQuery] TransactionQuery query)
        {
            var csvBytes = await _transactionService.ExportTransactionsToCsvAsync(query);

            return File(csvBytes, "text/csv", "transactions.csv");
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilter filter)
        {

            return Ok(await _transactionService.GetFilteredTransactionsAsync(filter));
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateTransactionStatus([FromBody] UpdateTransactionStatusRequest request)
        {
            await _transactionService.UpdateTransactionStatusAsync(request.TransactionId, request.NewStatus);
            return Ok("Transaction status updated successfully.");
        }
    }
}
