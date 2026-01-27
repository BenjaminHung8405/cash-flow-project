using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CashFlow.API.Infrastructure.Security
{
    /// <summary>
    /// JWT token generation helper for multi-tenant authentication.
    /// </summary>
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token with tenant and user information.
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="username">The username</param>
        /// <param name="tenantId">The tenant ID</param>
        /// <param name="roles">Optional roles for the user</param>
        /// <param name="expirationMinutes">Token expiration in minutes (default: 60)</param>
        /// <returns>JWT token string</returns>
        public string GenerateToken(
            string userId,
            string username,
            Guid tenantId,
            IEnumerable<string> roles = null,
            int expirationMinutes = 60)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim("tenantId", tenantId.ToString()),
                new Claim("userId", userId)
            };

            // Add roles if provided
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
