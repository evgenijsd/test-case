using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_case.api.Interfaces;

namespace test_case.api.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("import")]
        [Authorize]
        public async Task<IActionResult> ImportTransactions(IFormFile file)
        {
            await _transactionService.ImportTransactionsAsync(file);
            return Ok("Transactions imported successfully.");
        }

        [HttpPost("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("Transactions imported successfully.");
        }
    }
}
