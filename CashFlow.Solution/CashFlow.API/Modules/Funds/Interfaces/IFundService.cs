using CashFlow.API.Modules.Funds.DTOs;

namespace CashFlow.API.Modules.Funds.Interfaces
{
    /// <summary>
    /// Interface cho Fund Service - Định nghĩa business logic cho Funds
    /// </summary>
    public interface IFundService
    {
        /// <summary>
        /// Lấy tất cả quỹ của tenant hiện tại
        /// </summary>
        Task<IEnumerable<FundDto>> GetAllFundsAsync();

        /// <summary>
        /// Lấy quỹ theo ID
        /// </summary>
        Task<FundDto> GetFundByIdAsync(Guid id);

        /// <summary>
        /// Tạo quỹ mới
        /// </summary>
        Task<FundDto> CreateFundAsync(CreateFundRequest request);

        /// <summary>
        /// Cập nhật quỹ
        /// </summary>
        Task<FundDto> UpdateFundAsync(Guid id, UpdateFundRequest request);

        /// <summary>
        /// Xóa quỹ (soft delete)
        /// </summary>
        Task<bool> DeleteFundAsync(Guid id);

        /// <summary>
        /// Lấy tóm tắt số dư theo loại
        /// </summary>
        Task<IEnumerable<FundSummaryDto>> GetFundSummaryAsync();

        /// <summary>
        /// Lấy audit trail của quỹ
        /// </summary>
        Task<IEnumerable<object>> GetFundAuditTrailAsync(Guid fundId);
    }
}
