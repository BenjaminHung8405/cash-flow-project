using CashFlow.API.Data;
using CashFlow.API.Domain.Entities;
using CashFlow.API.Infrastructure.Services;
using CashFlow.API.Modules.Funds.DTOs;
using CashFlow.API.Modules.Funds.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.API.Modules.Funds.Services
{
    /// <summary>
    /// Implementation của Fund Service
    /// Xử lý business logic cho Funds (tài khoản tài chính)
    /// </summary>
    public class FundService : IFundService
    {
        private readonly AppDbContext _context;
        private readonly ITenantService _tenantService;
        private readonly ILogger<FundService> _logger;

        public FundService(AppDbContext context, ITenantService tenantService, ILogger<FundService> logger)
        {
            _context = context;
            _tenantService = tenantService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy tất cả quỹ của tenant hiện tại
        /// Query tự động filter theo TenantId
        /// </summary>
        public async Task<IEnumerable<FundDto>> GetAllFundsAsync()
        {
            try
            {
                var currentTenantId = _tenantService.GetCurrentTenantId();
                _logger.LogInformation($"Retrieving funds for tenant: {currentTenantId}");

                var funds = await _context.Funds
                    .AsNoTracking()
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.FundName)
                    .Select(f => new FundDto
                    {
                        Id = f.Id,
                        FundName = f.FundName,
                        FundType = f.FundType,
                        AccountNumber = f.AccountNumber,
                        CurrentBalance = f.CurrentBalance,
                        IsActive = f.IsActive,
                        CreatedAt = f.CreatedAt,
                        CreatedBy = f.CreatedBy
                    })
                    .ToListAsync();

                return funds;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving funds: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy quỹ theo ID
        /// </summary>
        public async Task<FundDto> GetFundByIdAsync(Guid id)
        {
            try
            {
                var fund = await _context.Funds
                    .AsNoTracking()
                    .Where(f => f.Id == id)
                    .Select(f => new FundDto
                    {
                        Id = f.Id,
                        FundName = f.FundName,
                        FundType = f.FundType,
                        AccountNumber = f.AccountNumber,
                        CurrentBalance = f.CurrentBalance,
                        IsActive = f.IsActive,
                        CreatedAt = f.CreatedAt,
                        CreatedBy = f.CreatedBy
                    })
                    .FirstOrDefaultAsync();

                return fund;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving fund {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Tạo quỹ mới
        /// TenantId tự động inject trong AppDbContext.SaveChangesAsync
        /// </summary>
        public async Task<FundDto> CreateFundAsync(CreateFundRequest request)
        {
            try
            {
                var fund = new Fund
                {
                    FundName = request.FundName,
                    FundType = request.FundType,
                    AccountNumber = request.AccountNumber,
                    CurrentBalance = request.InitialBalance,
                    IsActive = true
                    // TenantId, CreatedAt, CreatedBy được tự động inject
                };

                _context.Funds.Add(fund);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Fund created: {fund.Id}");

                return new FundDto
                {
                    Id = fund.Id,
                    FundName = fund.FundName,
                    FundType = fund.FundType,
                    AccountNumber = fund.AccountNumber,
                    CurrentBalance = fund.CurrentBalance,
                    IsActive = fund.IsActive,
                    CreatedAt = fund.CreatedAt,
                    CreatedBy = fund.CreatedBy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating fund: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cập nhật quỹ
        /// Tự động tạo AuditLog với old/new values
        /// </summary>
        public async Task<FundDto> UpdateFundAsync(Guid id, UpdateFundRequest request)
        {
            try
            {
                var fund = await _context.Funds.FindAsync(id);
                if (fund == null)
                {
                    return null;
                }

                // Cập nhật các trường nếu được cung cấp
                if (!string.IsNullOrEmpty(request.FundName))
                    fund.FundName = request.FundName;
                if (!string.IsNullOrEmpty(request.FundType))
                    fund.FundType = request.FundType;
                if (!string.IsNullOrEmpty(request.AccountNumber))
                    fund.AccountNumber = request.AccountNumber;
                if (request.IsActive.HasValue)
                    fund.IsActive = request.IsActive.Value;

                _context.Funds.Update(fund);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Fund updated: {id}");

                return new FundDto
                {
                    Id = fund.Id,
                    FundName = fund.FundName,
                    FundType = fund.FundType,
                    AccountNumber = fund.AccountNumber,
                    CurrentBalance = fund.CurrentBalance,
                    IsActive = fund.IsActive,
                    CreatedAt = fund.CreatedAt,
                    CreatedBy = fund.CreatedBy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating fund {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Xóa quỹ (soft delete - set IsActive = false)
        /// </summary>
        public async Task<bool> DeleteFundAsync(Guid id)
        {
            try
            {
                var fund = await _context.Funds.FindAsync(id);
                if (fund == null)
                {
                    return false;
                }

                fund.IsActive = false;
                _context.Funds.Update(fund);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Fund deleted (soft): {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting fund {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy tóm tắt số dư theo loại quỹ
        /// </summary>
        public async Task<IEnumerable<FundSummaryDto>> GetFundSummaryAsync()
        {
            try
            {
                var summary = await _context.Funds
                    .Where(f => f.IsActive)
                    .GroupBy(f => f.FundType)
                    .Select(g => new FundSummaryDto
                    {
                        FundType = g.Key,
                        Count = g.Count(),
                        TotalBalance = g.Sum(f => f.CurrentBalance)
                    })
                    .ToListAsync();

                return summary;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving fund summary: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy lịch sử thay đổi của quỹ (Audit Trail)
        /// </summary>
        public async Task<IEnumerable<object>> GetFundAuditTrailAsync(Guid fundId)
        {
            try
            {
                var auditLogs = await _context.AuditLogs
                    .Where(a => a.TableName == nameof(Fund) && a.RecordId == fundId.ToString())
                    .OrderByDescending(a => a.Timestamp)
                    .Select(a => new
                    {
                        a.Id,
                        a.Action,
                        a.UserId,
                        a.Timestamp,
                        OldValues = a.OldValues,
                        NewValues = a.NewValues
                    })
                    .ToListAsync();

                return auditLogs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving audit trail for fund {fundId}: {ex.Message}");
                throw;
            }
        }
    }
}
