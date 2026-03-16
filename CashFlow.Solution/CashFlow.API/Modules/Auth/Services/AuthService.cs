using BCrypt.Net;
using CashFlow.API.Data;
using CashFlow.API.Domain.Entities;
using CashFlow.API.Infrastructure.Services;
using CashFlow.API.Modules.Auth.DTOs;
using CashFlow.API.Modules.Auth.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlow.API.Modules.Auth.Services
{
    /// <summary>
    /// Implementation của Auth Service
    /// Xử lý logic xác thực, mã hóa password, tạo JWT Token
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly DatabaseService _db;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _efContext;
        public AuthService(DatabaseService db, IConfiguration config, ILogger<AuthService> logger, AppDbContext efContext)
        {
            _db = db;
            _config = config;
            _logger = logger;
            _efContext = efContext;
        }
        public async Task<UserInfoDto> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.companyName) || string.IsNullOrEmpty(request.Email))
            {
                throw new UnauthorizedAccessException("Username, password, email, and CompanyName are required.");
            }

            try
            {
            // Mở transaction bằng EF Core
                using var transaction = await _efContext.Database.BeginTransactionAsync();
                // Kiểm tra user có tồn tại không
                var userExists = await _efContext.Users
                    .IgnoreQueryFilters()
                    .AnyAsync(u => u.Username == request.Username || u.Email == request.Email);

                if (userExists)
                {
                    throw new Exception("Username or Email already exists.");
                }

                // 1. Tạo Tenant mới
                var newTenantId = Guid.NewGuid();
                var newTenant = new Tenant
                {
                    Id = newTenantId,
                    CompanyName = request.companyName,
                    IsActive = true
                };
                _efContext.Tenants.Add(newTenant);

                // 2. Tạo Role "Admin" cho Tenant mới
                var adminRoleId = Guid.NewGuid();
                var adminRole = new Role
                {
                    Id = adminRoleId,
                    TenantId = newTenantId,
                    RoleName = "Admin",
                    Description = "Tenant Administrator",
                    IsActive = true
                };
                _efContext.Roles.Add(adminRole);

                // 3. Tạo User và liên kết
                var userId = Guid.NewGuid();
                var newUser = new User
                {
                    Id = userId,
                    TenantId = newTenantId,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    RoleId = adminRoleId,
                    IsActive = true
                };
                _efContext.Users.Add(newUser);

                // Lưu thay đổi vào DB
                await _efContext.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                return new UserInfoDto
                {
                    UserId = userId.ToString(),
                    Username = newUser.Username,
                    Email = newUser.Email,
                    companyName = newTenant.CompanyName,
                    Role = adminRole.RoleName,
                    TenantId = newTenantId.ToString()
                };
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
                _logger.LogError($"Lỗi đăng ký user: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Đăng nhập: 
        /// 1. Kiểm tra username/password trong DB
        /// 2. Nếu đúng, lấy UserId, TenantId, Role
        /// 3. Tạo JWT Token gắn những thông tin này
        /// 4. Trả về Client
        /// </summary>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new UnauthorizedAccessException("Username và Password là bắt buộc");
            }

            try
            {
                
                // 1. Query database để kiếm user
                using (var conn = _db.CreateConnection())
                {
                    string sql = @"
                        SELECT u.Id, u.TenantId, t.CompanyName, u.Username, u.PasswordHash, r.RoleName 
                        FROM Users u
                        LEFT JOIN Roles r ON u.RoleId = r.Id
                        LEFT JOIN Tenants t ON u.TenantId = t.Id
                        WHERE u.Username = @Username AND u.IsActive = 1";

                    var user = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new
                    {
                        Username = request.Username,
                    });

                    if (user == null)
                    {
                        _logger.LogWarning($"Đăng nhập thất bại: Username '{request.Username}' không tồn tại hoặc password sai");
                        throw new UnauthorizedAccessException("Sai tài khoản hoặc mật khẩu.");
                    }
                    // 2. Dùng BCrypt Verify để kiểm tra mật khẩu nhập vào với mã băm trong DB
                    // Cú pháp: Verify(chuỗi_chưa_mã_hóa, chuỗi_đã_mã_hóa_trong_db)
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, (string)user.PasswordHash);

                    if (!isPasswordValid)
                    {
                        _logger.LogWarning($"Đăng nhập thất bại: Sai mật khẩu cho user '{request.Username}'");
                        throw new UnauthorizedAccessException("Sai tài khoản hoặc mật khẩu.");
                    }
                    // 3. Tạo Token
                    string token = GenerateJwtToken(user);

                    // 4. Trả về Response
                    var response = new LoginResponse
                    {
                        Token = token,
                        companyName = user.CompanyName,
                        Role = user.RoleName ?? "User",
                        TenantId = user.TenantId.ToString()
                    };

                    _logger.LogInformation($"Đăng nhập thành công cho user: {request.Username}");
                    return response;
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi trong LoginAsync: {ex.Message}");
                throw new Exception("Lỗi khi xử lý đăng nhập", ex);
            }
        }



        public async Task<LoginResponse> RefreshTokenAsync(string oldToken)
        {
            throw new NotImplementedException("Refresh token feature chưa được implement");
        }

        public async Task<UserInfoDto> GetUserByIdAsync(string userId)
        {
            try
            {
                using (var conn = _db.CreateConnection())
                {
                    string sql = @"
                        SELECT u.Id as UserId, u.Username, t.CompanyName as companyName, u.Email, u.TenantId, r.RoleName as Role
                        FROM Users u
                        LEFT JOIN Roles r ON u.RoleId = r.Id
                        LEFT JOIN Tenants t ON u.TenantId = t.Id
                        WHERE u.Id = @UserId";

                    var user = await conn.QueryFirstOrDefaultAsync<UserInfoDto>(sql, new { UserId = userId });

                    if (user == null)
                    {
                        throw new Exception($"User với ID {userId} không tồn tại");
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi lấy thông tin user {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task LogoutAsync(string userId)
        {
            // TODO: Implement logic logout (nếu cần - ví dụ: invalidate token)
            _logger.LogInformation($"User {userId} đã đăng xuất");
            await Task.CompletedTask;
        }

        /// <summary>
        /// Tạo JWT Token
        /// Token chứa: UserId, TenantId, BranchId, Role
        /// Client sử dụng token này trong mỗi request
        /// </summary>
        private string GenerateJwtToken(dynamic user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("TenantId", user.TenantId.ToString()),
                new Claim(ClaimTypes.Role, user.RoleName ?? "User"),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
