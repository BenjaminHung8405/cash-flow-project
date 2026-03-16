namespace CashFlow.API.Modules.Funds.DTOs
{
    /// <summary>
    /// DTO hiển thị thông tin quỹ
    /// Luồng: API → Client
    /// </summary>
    public class FundDto
    {
        public Guid Id { get; set; }
        public string FundName { get; set; }
        public string FundType { get; set; } // "CASH", "BANK"
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu tạo quỹ mới
    /// Luồng: Client → API
    /// </summary>
    public class CreateFundRequest
    {
        public string FundName { get; set; }
        public string FundType { get; set; } // "CASH", "BANK"
        public string AccountNumber { get; set; }
        public decimal InitialBalance { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu cập nhật quỹ
    /// Luồng: Client → API
    /// </summary>
    public class UpdateFundRequest
    {
        public string FundName { get; set; }
        public string FundType { get; set; }
        public string AccountNumber { get; set; }
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO tóm tắt số dư theo loại quỹ
    /// Luồng: API → Client
    /// </summary>
    public class FundSummaryDto
    {
        public string FundType { get; set; }
        public int Count { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
