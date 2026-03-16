using CashFlow.API.Data;
using CashFlow.API.Domain.Entities;
using CashFlow.API.Infrastructure.Services;
using CashFlow.API.Modules.Funds.DTOs;
using CashFlow.API.Modules.Funds.Interfaces;
using Dapper;
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
        private readonly DatabaseService _db;

        public FundService(AppDbContext context, ITenantService tenantService, ILogger<FundService> logger, DatabaseService db)
        {
            _context = context;
            _tenantService = tenantService;
            _logger = logger;
            _db = db;
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

                using var conn = _db.CreateConnection();
                string sql = @"
                    SELECT Id, FundName, FundType, AccountNumber, CurrentBalance, IsActive, CreatedAt, CreatedBy 
                    FROM Funds 
                    WHERE TenantId = @TenantId AND IsActive = 1 
                    ORDER BY FundName";

                var funds = await conn.QueryAsync<FundDto>(sql, new { TenantId = currentTenantId });
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
                var currentTenantId = _tenantService.GetCurrentTenantId();
                using var conn = _db.CreateConnection();
                string sql = @"
                    SELECT Id, FundName, FundType, AccountNumber, CurrentBalance, IsActive, CreatedAt, CreatedBy 
                    FROM Funds 
                    WHERE Id = @Id AND TenantId = @TenantId";

                var fund = await conn.QueryFirstOrDefaultAsync<FundDto>(sql, new { Id = id, TenantId = currentTenantId });
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
                var currentTenantId = _tenantService.GetCurrentTenantId();
                using var conn = _db.CreateConnection();
                string sql = @"
                    SELECT FundType, COUNT(*) as Count, SUM(CurrentBalance) as TotalBalance 
                    FROM Funds 
                    WHERE TenantId = @TenantId AND IsActive = 1 
                    GROUP BY FundType";

                var summary = await conn.QueryAsync<FundSummaryDto>(sql, new { TenantId = currentTenantId });
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
                var currentTenantId = _tenantService.GetCurrentTenantId();
                using var conn = _db.CreateConnection();
                string sql = @"
                    SELECT Id, Action, UserId, Timestamp, OldValues, NewValues 
                    FROM AuditLogs 
                    WHERE TableName = 'Fund' AND RecordId = @RecordId AND TenantId = @TenantId 
                    ORDER BY Timestamp DESC";

                var auditLogs = await conn.QueryAsync<dynamic>(sql, new { RecordId = fundId.ToString(), TenantId = currentTenantId });
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
