using CashFlow.API.Infrastructure.Services;
using CashFlow.API.Modules.Auth.DTOs;
using CashFlow.API.Modules.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Modules.Auth.Controllers
{
    /// <summary>
    /// Authentication Controller - Xử lý đăng nhập, đăng xuất, token refresh
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IConfiguration config, ILogger<AuthController> logger)
        {
            _authService = authService;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Đăng nhập: Client gửi username + password
        /// Server trả về JWT Token chứa TenantId, UserId, Role
        /// </summary>
        /// <param name="request">LoginRequest: { Username, Password }</param>
        /// <returns>LoginResponse: { Token, FullName, Role }</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login request không hợp lệ");
                return BadRequest(new { message = "Username và Password là bắt buộc" });
            }

            try
            {
                var response = await _authService.LoginAsync(request);
                _logger.LogInformation($"User {request.Username} đăng nhập thành công");
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Đăng nhập thất bại: {ex.Message}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi đăng nhập: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi server khi xử lý đăng nhập" });
            }
        }

        /// <summary>
        /// Đăng ký user mới (nếu cần)
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                _logger.LogInformation($"User {request.Username} đăng ký thành công");
                return Ok(new { message = "Đăng ký thành công", data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi đăng ký: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Refresh Token (mở rộng thời gian sử dụng)
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request.Token);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin user hiện tại (từ token)
        /// </summary>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Không tìm thấy userId trong token" });
                }

                var user = await _authService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi lấy thông tin user: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi server" });
            }
        }
    }
}
