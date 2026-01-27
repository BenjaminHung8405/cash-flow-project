using CashFlow.API.Data;
using CashFlow.API.Domain.Entities;
using CashFlow.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.API.Controllers
{
    /// <summary>
    /// Example controller demonstrating multi-tenant fund management.
    /// Shows how to use the AppDbContext with automatic multi-tenancy support.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FundsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITenantService _tenantService;
        private readonly ILogger<FundsController> _logger;

        public FundsController(
            AppDbContext context,
            ITenantService tenantService,
            ILogger<FundsController> logger)
        {
            _context = context;
            _tenantService = tenantService;
            _logger = logger;
        }

        /// <summary>
        /// Get all funds for the current tenant.
        /// The query is automatically filtered by tenant ID through global query filters.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fund>>> GetFunds()
        {
            try
            {
                var currentTenantId = _tenantService.GetCurrentTenantId();
                _logger.LogInformation($"Retrieving funds for tenant: {currentTenantId}");

                var funds = await _context.Funds
                    .AsNoTracking()
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.FundName)
                    .ToListAsync();

                return Ok(funds);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Tenant resolution error: {ex.Message}");
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }

        /// <summary>
        /// Get a specific fund by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Fund>> GetFund(Guid id)
        {
            try
            {
                var fund = await _context.Funds.FindAsync(id);

                if (fund == null)
                {
                    return NotFound(new { message = "Fund not found" });
                }

                return Ok(fund);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }

        /// <summary>
        /// Create a new fund.
        /// TenantId and CreatedAt are automatically injected before saving.
        /// An AuditLog entry is automatically created.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Fund>> CreateFund([FromBody] CreateFundRequest request)
        {
            try
            {
                var currentUserId = _tenantService.GetCurrentUserId();

                var fund = new Fund
                {
                    FundName = request.FundName,
                    FundType = request.FundType,
                    AccountNumber = request.AccountNumber,
                    CurrentBalance = request.InitialBalance,
                    IsActive = true
                    // TenantId, CreatedAt, and CreatedBy are automatically set in SaveChangesAsync
                };

                _context.Funds.Add(fund);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Fund created: {fund.Id} by user: {currentUserId}");

                return CreatedAtAction(nameof(GetFund), new { id = fund.Id }, fund);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Tenant resolution error: {ex.Message}");
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }

        /// <summary>
        /// Update an existing fund.
        /// Changes are automatically logged to AuditLog.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFund(Guid id, [FromBody] UpdateFundRequest request)
        {
            try
            {
                var fund = await _context.Funds.FindAsync(id);

                if (fund == null)
                {
                    return NotFound(new { message = "Fund not found" });
                }

                fund.FundName = request.FundName ?? fund.FundName;
                fund.FundType = request.FundType ?? fund.FundType;
                fund.AccountNumber = request.AccountNumber ?? fund.AccountNumber;
                fund.IsActive = request.IsActive ?? fund.IsActive;

                _context.Funds.Update(fund);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Fund updated: {id}");

                return Ok(new { message = "Fund updated successfully", data = fund });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }

        /// <summary>
        /// Delete a fund (soft delete by setting IsActive to false).
        /// A DELETE action is recorded in AuditLog.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFund(Guid id)
        {
            try
            {
                var fund = await _context.Funds.FindAsync(id);

                if (fund == null)
                {
                    return NotFound(new { message = "Fund not found" });
                }

                fund.IsActive = false;

                _context.Funds.Update(fund);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Fund deleted: {id}");

                return Ok(new { message = "Fund deleted successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }

        /// <summary>
        /// Get fund balance summary for the current tenant.
        /// </summary>
        [HttpGet("summary/balance")]
        public async Task<ActionResult<object>> GetBalanceSummary()
        {
            try
            {
                var summary = await _context.Funds
                    .Where(f => f.IsActive)
                    .GroupBy(f => f.FundType)
                    .Select(g => new
                    {
                        FundType = g.Key,
                        Count = g.Count(),
                        TotalBalance = g.Sum(f => f.CurrentBalance)
                    })
                    .ToListAsync();

                return Ok(summary);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }

        /// <summary>
        /// Get audit trail for a specific fund.
        /// Shows all changes made to the fund.
        /// </summary>
        [HttpGet("{id}/audit-trail")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetFundAuditTrail(Guid id)
        {
            try
            {
                var currentTenantId = _tenantService.GetCurrentTenantId();

                var auditLogs = await _context.AuditLogs
                    .Where(a => a.TableName == nameof(Fund) && a.RecordId == id.ToString())
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();

                return Ok(auditLogs);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }
    }

    /// <summary>
    /// DTO for creating a fund
    /// </summary>
    public class CreateFundRequest
    {
        public string FundName { get; set; }
        public string FundType { get; set; } // "CASH", "BANK"
        public string AccountNumber { get; set; }
        public decimal InitialBalance { get; set; }
    }

    /// <summary>
    /// DTO for updating a fund
    /// </summary>
    public class UpdateFundRequest
    {
        public string FundName { get; set; }
        public string FundType { get; set; }
        public string AccountNumber { get; set; }
        public bool? IsActive { get; set; }
    }
}
