using CashFlow.API.Modules.Auth.DTOs;

namespace CashFlow.API.Modules.Auth.Interfaces
{
    /// <summary>
    /// Interface cho Auth Service - Định nghĩa các operation xác thực
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Đăng nhập user
        /// </summary>
        /// <param name="request">Thông tin đăng nhập (Username, Password)</param>
        /// <returns>LoginResponse với JWT Token</returns>
        /// <exception cref="UnauthorizedAccessException">Khi sai username hoặc password</exception>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Đăng ký user mới
        /// </summary>
        /// <param name="request">Thông tin đăng ký</param>
        /// <returns>Thông tin user vừa tạo</returns>
        Task<UserInfoDto> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Refresh Token - Cấp token mới
        /// </summary>
        /// <param name="oldToken">Token cũ</param>
        /// <returns>Token mới</returns>
        Task<LoginResponse> RefreshTokenAsync(string oldToken);

        /// <summary>
        /// Lấy thông tin user theo UserId
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns>Thông tin user</returns>
        Task<UserInfoDto> GetUserByIdAsync(string userId);

        /// <summary>
        /// Đăng xuất (nếu cần)
        /// </summary>
        Task LogoutAsync(string userId);
    }
}
