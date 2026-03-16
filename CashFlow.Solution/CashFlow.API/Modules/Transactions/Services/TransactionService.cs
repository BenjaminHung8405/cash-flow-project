using CashFlow.API.Data;
using CashFlow.API.Domain.Entities;
using CashFlow.API.Infrastructure.Services;
using CashFlow.API.Modules.Transactions.DTOs;
using CashFlow.API.Modules.Transactions.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.API.Modules.Transactions.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly DatabaseService _db;
        private readonly ITenantService _tenantService;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(AppDbContext context, DatabaseService db, ITenantService tenantService, ILogger<TransactionService> logger)
        {
            _context = context;
            _db = db;
            _tenantService = tenantService;
            _logger = logger;
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var currentTenantId = _tenantService.GetCurrentTenantId();
                var fund = await _context.Funds.FirstOrDefaultAsync(f => f.Id == request.FundId && f.IsActive);
                
                if (fund == null)
                {
                    throw new Exception("Fund not found or inactive.");
                }

                if (request.TransactionType == "OUT" && fund.CurrentBalance < request.Amount)
                {
                    throw new Exception("Insufficient fund balance for OUT transaction.");
                }

                if (request.TransactionType == "IN")
                {
                    fund.CurrentBalance += request.Amount;
                }
                else if (request.TransactionType == "OUT")
                {
                    fund.CurrentBalance -= request.Amount;
                }

                var newTransactionId = Guid.NewGuid();
                var newTransaction = new Transaction
                {
                    Id = newTransactionId,
                    VoucherCode = request.VoucherCode,
                    TransactionType = request.TransactionType,
                    Amount = request.Amount,
                    FundId = request.FundId,
                    Description = request.Description,
                    CategoryName = request.CategoryName,
                    TransactionDate = request.TransactionDate ?? DateTime.UtcNow,
                };

                _context.Funds.Update(fund);
                _context.Transactions.Add(newTransaction);
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new TransactionDto
                {
                    Id = newTransaction.Id,
                    VoucherCode = newTransaction.VoucherCode,
                    TransactionType = newTransaction.TransactionType,
                    Amount = newTransaction.Amount,
                    FundId = newTransaction.FundId,
                    FundName = fund.FundName,
                    Description = newTransaction.Description,
                    CategoryName = newTransaction.CategoryName,
                    TransactionDate = newTransaction.TransactionDate,
                    CreatedAt = newTransaction.CreatedAt,
                    CreatedBy = newTransaction.CreatedBy
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error creating transaction: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TransactionDto>> GetRecentTransactionsAsync(int limit = 10)
        {
            try
            {
                var tenantId = _tenantService.GetCurrentTenantId();
                using var conn = _db.CreateConnection();
                string sql = $@"
                    SELECT TOP(@Limit) t.Id, t.VoucherCode, t.TransactionType, t.Amount, t.FundId, f.FundName, 
                           t.Description, t.CategoryName, t.TransactionDate, t.CreatedAt, t.CreatedBy
                    FROM Transactions t
                    INNER JOIN Funds f ON t.FundId = f.Id
                    WHERE t.TenantId = @TenantId
                    ORDER BY t.TransactionDate DESC, t.CreatedAt DESC";

                var transactions = await conn.QueryAsync<TransactionDto>(sql, new { TenantId = tenantId, Limit = limit });
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving recent transactions: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DonutChartDataDto>> GetDonutChartDataAsync()
        {
            try
            {
                var tenantId = _tenantService.GetCurrentTenantId();
                using var conn = _db.CreateConnection();
                string sql = @"
                    SELECT CategoryName, SUM(Amount) as TotalAmount
                    FROM Transactions
                    WHERE TenantId = @TenantId AND TransactionType = 'OUT'
                    GROUP BY CategoryName";
                
                var data = await conn.QueryAsync<DonutChartDataDto>(sql, new { TenantId = tenantId });
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving donut chart data: {ex.Message}");
                throw;
            }
        }
    }
}
