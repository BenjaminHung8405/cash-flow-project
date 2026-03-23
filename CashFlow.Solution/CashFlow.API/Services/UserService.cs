using CashFlow.API.DTOs;
using CashFlow.API.Services;
using Dapper;
using System.Security.Cryptography;
using System.Text;

namespace CashFlow.API.Services
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserByIdAsync(int userId);
        Task<UserListDto> GetAllUsersAsync(int tenantId, int pageNumber = 1, int pageSize = 10);
        Task<int> CreateUserAsync(UserCreateDto dto);
        Task<bool> UpdateUserAsync(int userId, UserUpdateDto dto);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> CheckUsernameExistsAsync(string username, int tenantId);
    }

    public class UserService : IUserService
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<UserService> _logger;

        public UserService(DatabaseService databaseService, ILogger<UserService> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<UserViewModel> GetUserByIdAsync(int userId)
        {
            try
            {
                using (var conn = _databaseService.CreateConnection())
                {
                    const string sql = @"
                        SELECT 
                            u.UserId, u.Username, u.FullName, 
                            u.RoleId, r.RoleName, u.TenantId, 
                            u.BranchId, u.IsActive, u.CreatedDate
                        FROM Users u
                        INNER JOIN Roles r ON u.RoleId = r.RoleId
                        WHERE u.UserId = @UserId";

                    var user = await conn.QueryFirstOrDefaultAsync<UserViewModel>(sql, new { UserId = userId });
                    return user ?? throw new InvalidOperationException($"User with ID {userId} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user {userId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all users for a tenant with pagination
        /// </summary>
        public async Task<UserListDto> GetAllUsersAsync(int tenantId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                using (var conn = _databaseService.CreateConnection())
                {
                    // Get total count
                    const string countSql = "SELECT COUNT(*) FROM Users WHERE TenantId = @TenantId";
                    int totalCount = await conn.QueryFirstOrDefaultAsync<int>(countSql, new { TenantId = tenantId });

                    // Get paginated data
                    const string dataSql = @"
                        SELECT 
                            u.UserId, u.Username, u.FullName, 
                            u.RoleId, r.RoleName, u.TenantId, 
                            u.BranchId, u.IsActive, u.CreatedDate
                        FROM Users u
                        INNER JOIN Roles r ON u.RoleId = r.RoleId
                        WHERE u.TenantId = @TenantId
                        ORDER BY u.CreatedDate DESC
                        OFFSET (@PageNumber - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";

                    var users = await conn.QueryAsync<UserViewModel>(dataSql,
                        new { TenantId = tenantId, PageNumber = pageNumber, PageSize = pageSize });

                    return new UserListDto
                    {
                        TotalCount = totalCount,
                        Users = users.ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving users for tenant {tenantId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Create a new user account
        /// </summary>
        public async Task<int> CreateUserAsync(UserCreateDto dto)
        {
            try
            {
                // Validate username uniqueness
                if (await CheckUsernameExistsAsync(dto.Username, dto.TenantId))
                {
                    throw new InvalidOperationException($"Username '{dto.Username}' already exists for this tenant.");
                }

                string passwordHash = ComputePasswordHash(dto.Password);

                using (var conn = _databaseService.CreateConnection())
                {
                    const string sql = @"
                        INSERT INTO Users 
                        (TenantId, BranchId, RoleId, Username, PasswordHash, FullName, IsActive)
                        VALUES 
                        (@TenantId, @BranchId, @RoleId, @Username, @PasswordHash, @FullName, @IsActive);
                        
                        SELECT CAST(SCOPE_IDENTITY() as int)";

                    var userId = await conn.QueryFirstOrDefaultAsync<int>(sql, new
                    {
                        dto.TenantId,
                        dto.BranchId,
                        dto.RoleId,
                        dto.Username,
                        PasswordHash = passwordHash,
                        dto.FullName,
                        dto.IsActive
                    });

                    _logger.LogInformation($"User created successfully. UserId: {userId}, Username: {dto.Username}");
                    return userId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Update user information
        /// </summary>
        public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto dto)
        {
            try
            {
                using (var conn = _databaseService.CreateConnection())
                {
                    const string sql = @"
                        UPDATE Users
                        SET 
                            FullName = @FullName,
                            RoleId = @RoleId,
                            BranchId = @BranchId,
                            IsActive = @IsActive
                        WHERE UserId = @UserId";

                    var result = await conn.ExecuteAsync(sql, new
                    {
                        userId,
                        dto.FullName,
                        dto.RoleId,
                        dto.BranchId,
                        dto.IsActive
                    });

                    _logger.LogInformation($"User updated successfully. UserId: {userId}");
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user {userId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Delete a user (soft delete by setting IsActive to false)
        /// </summary>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                using (var conn = _databaseService.CreateConnection())
                {
                    const string sql = "UPDATE Users SET IsActive = 0 WHERE UserId = @UserId";
                    var result = await conn.ExecuteAsync(sql, new { UserId = userId });

                    _logger.LogInformation($"User marked as inactive. UserId: {userId}");
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user {userId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Check if username already exists for the tenant
        /// </summary>
        public async Task<bool> CheckUsernameExistsAsync(string username, int tenantId)
        {
            try
            {
                using (var conn = _databaseService.CreateConnection())
                {
                    const string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND TenantId = @TenantId";
                    int count = await conn.QueryFirstOrDefaultAsync<int>(sql,
                        new { Username = username, TenantId = tenantId });

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking username existence: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Compute password hash using MD5 (for compatibility with seed data)
        /// Note: In production, consider using PBKDF2, Bcrypt, or Argon2
        /// </summary>
        private string ComputePasswordHash(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}