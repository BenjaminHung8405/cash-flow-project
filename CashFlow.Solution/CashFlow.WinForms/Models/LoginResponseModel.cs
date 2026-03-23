namespace CashFlow.WinForms.Models
{
    /// <summary>
    /// Response model từ API khi đăng nhập thành công
    /// </summary>
    public class LoginResponseModel
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
        public int TenantId { get; set; }
    }
}
