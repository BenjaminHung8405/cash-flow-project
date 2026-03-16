using CashFlow.API.Data;
using CashFlow.API.Domain.Entities;
using CashFlow.API.Infrastructure.Services;
using CashFlow.API.Modules.Funds.DTOs;
using CashFlow.API.Modules.Funds.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.API.Modules.Funds.Controllers
{
    /// <summary>
    /// Funds (Quỹ / Tài khoản tài chính) Controller
    /// Xử lý CRUD operations cho các tài khoản tiền mặt/ngân hàng
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FundsController : ControllerBase
    {
        private readonly IFundService _fundService;
        private readonly ILogger<FundsController> _logger;

        public FundsController(IFundService fundService, ILogger<FundsController> logger)
        {
            _fundService = fundService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả các quỹ của tenant hiện tại
        /// Query tự động filter theo TenantId từ JWT
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundDto>>> GetFunds()
        {
            try
            {
                var funds = await _fundService.GetAllFundsAsync();
                return Ok(funds);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Tenant resolution error: {ex.Message}");
                return Unauthorized(new { message = "Invalid tenant context" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting funds: {ex.Message}");
                return StatusCode(500, new { message = "Error retrieving funds" });
            }
        }

        /// <summary>
        /// Lấy chi tiết một quỹ cụ thể
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FundDto>> GetFund(Guid id)
        {
            try
            {
                var fund = await _fundService.GetFundByIdAsync(id);
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
        /// Tạo quỹ mới
        /// TenantId tự động inject từ JWT token
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FundDto>> CreateFund([FromBody] CreateFundRequest request)
        {
            try
            {
                var fund = await _fundService.CreateFundAsync(request);
                _logger.LogInformation($"Fund created: {fund.Id}");
                return CreatedAtAction(nameof(GetFund), new { id = fund.Id }, fund);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Tenant resolution error: {ex.Message}");
                return Unauthorized(new { message = "Invalid tenant context" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating fund: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin quỹ
        /// Tự động tạo audit log
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFund(Guid id, [FromBody] UpdateFundRequest request)
        {
            try
            {
                var fund = await _fundService.UpdateFundAsync(id, request);
                if (fund == null)
                {
                    return NotFound(new { message = "Fund not found" });
                }
                _logger.LogInformation($"Fund updated: {id}");
                return Ok(new { message = "Fund updated successfully", data = fund });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating fund: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Xóa quỹ (soft delete - chỉ set IsActive = false)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFund(Guid id)
        {
            try
            {
                var success = await _fundService.DeleteFundAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Fund not found" });
                }
                _logger.LogInformation($"Fund deleted: {id}");
                return Ok(new { message = "Fund deleted successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting fund: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tóm tắt số dư tài khoản theo loại
        /// </summary>
        [HttpGet("summary/balance")]
        public async Task<ActionResult<IEnumerable<FundSummaryDto>>> GetBalanceSummary()
        {
            try
            {
                var summary = await _fundService.GetFundSummaryAsync();
                return Ok(summary);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }

        /// <summary>
        /// Lấy lịch sử thay đổi của quỹ (Audit Trail)
        /// </summary>
        [HttpGet("{id}/audit-trail")]
        public async Task<ActionResult<IEnumerable<object>>> GetFundAuditTrail(Guid id)
        {
            try
            {
                var auditLogs = await _fundService.GetFundAuditTrailAsync(id);
                return Ok(auditLogs);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "Invalid tenant context" });
            }
        }
    }
}
