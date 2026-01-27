namespace CashFlow.API.DTOs
{
    // Dữ liệu client gửi lên để đăng nhập
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Dữ liệu server trả về khi đăng nhập thành công
    public class LoginResponse
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}