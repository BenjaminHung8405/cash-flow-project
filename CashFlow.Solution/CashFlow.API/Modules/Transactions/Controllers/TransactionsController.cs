using CashFlow.API.Modules.Transactions.DTOs;
using CashFlow.API.Modules.Transactions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Modules.Transactions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var transaction = await _transactionService.CreateTransactionAsync(request);
                return Ok(new { message = "Transaction created successfully", data = transaction });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating transaction: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetRecentTransactions([FromQuery] int limit = 10)
        {
            try
            {
                var transactions = await _transactionService.GetRecentTransactionsAsync(limit);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving recent transactions" });
            }
        }

        [HttpGet("donut-chart")]
        public async Task<ActionResult<IEnumerable<DonutChartDataDto>>> GetDonutChartData()
        {
            try
            {
                var data = await _transactionService.GetDonutChartDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving donut chart data" });
            }
        }
    }
}
