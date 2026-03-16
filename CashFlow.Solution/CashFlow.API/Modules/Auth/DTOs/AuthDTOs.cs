namespace CashFlow.API.Modules.Auth.DTOs
{
    /// <summary>
    /// DTO yêu cầu từ Client khi đăng nhập
    /// Luồng: Client → API
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// DTO phản hồi từ Server sau khi đăng nhập thành công
    /// Luồng: API → Client
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// JWT Token - Chứa: UserId, TenantId, BranchId, Role
        /// Client lưu vào localStorage/sessionStorage
        /// Gửi kèm mỗi request trong header: Authorization: Bearer {Token}
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Tên đầy đủ của user (hiển thị trên UI)
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Tên role của user (Admin, Manager, Staff, v.v.)
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// TenantId (nếu cần để client lưu)
        /// </summary>
        public string TenantId { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu đăng ký user mới
    /// </summary>
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// DTO yêu cầu refresh token
    /// </summary>
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
    }

    /// <summary>
    /// DTO thông tin user
    /// </summary>
    public class UserInfoDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string TenantId { get; set; }
    }
}
