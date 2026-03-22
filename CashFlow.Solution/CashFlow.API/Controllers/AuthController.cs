using CashFlow.API.DTOs;
using CashFlow.API.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CashFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseService _db;
        private readonly IConfiguration _config;

        public AuthController(DatabaseService db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Mã hóa mật khẩu client gửi lên sang MD5 (để khớp với seed data)
            string passwordHash = ComputeMD5(request.Password);

            // 2. Kiểm tra trong Database
            using (var conn = _db.CreateConnection())
            {
                string sql = @"
                    SELECT u.UserId, u.TenantId, u.BranchId, u.FullName, r.RoleName 
                    FROM Users u
                    JOIN Roles r ON u.RoleId = r.RoleId
                    WHERE u.Username = @Username AND u.PasswordHash = @PasswordHash AND u.IsActive = 1";

                var user = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new
                {
                    Username = request.Username,
                    PasswordHash = passwordHash
                });

                if (user == null)
                {
                    return Unauthorized("Sai tài khoản hoặc mật khẩu.");
                }

                // 3. Tạo Token (Gắn TenantId vào trong ruột Token)
                var token = GenerateJwtToken(user);

                return Ok(new LoginResponse
                {
                    Token = token,
                    FullName = user.FullName,
                    Role = user.RoleName
                });
            }
        }

        // Hàm tạo Token JWT
        private string GenerateJwtToken(dynamic user)
        {
            var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key configuration is missing");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("TenantId", user.TenantId.ToString()), // <--- QUAN TRỌNG NHẤT
                new Claim("BranchId", user.BranchId?.ToString() ?? ""),
                new Claim(ClaimTypes.Role, user.RoleName)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(24), // Token sống 24h
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Hàm phụ trợ mã hóa MD5
        private string ComputeMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes).ToLower(); // Trả về chuỗi hex chữ thường
            }
        }
    }
}