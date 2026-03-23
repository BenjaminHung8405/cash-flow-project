using System.ComponentModel.DataAnnotations;

namespace CashFlow.API.DTOs
{
    /// <summary>
    /// Dữ liệu client gửi lên để đăng nhập
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// Dữ liệu server trả về khi đăng nhập thành công
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
        public int TenantId { get; set; }
    }
}