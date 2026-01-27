using System.Security.Claims;

namespace CashFlow.API.Infrastructure.Services
{
    /// <summary>
    /// Implementation of the tenant resolution service.
    /// Extracts tenant ID from JWT token claims in the current HTTP request.
    /// </summary>
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the current tenant ID from JWT claims.
        /// </summary>
        public Guid GetCurrentTenantId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User == null)
            {
                throw new InvalidOperationException("HTTP context or user is not available.");
            }

            var tenantIdClaim = httpContext.User.FindFirst("tenantId") 
                ?? httpContext.User.FindFirst(ClaimTypes.GroupSid)
                ?? httpContext.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid");

            if (tenantIdClaim == null || !Guid.TryParse(tenantIdClaim.Value, out var tenantId))
            {
                throw new InvalidOperationException("TenantId claim not found or invalid in JWT token.");
            }

            return tenantId;
        }

        /// <summary>
        /// Gets the current user ID from JWT claims.
        /// </summary>
        public string GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User == null)
            {
                throw new InvalidOperationException("HTTP context or user is not available.");
            }

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                ?? httpContext.User.FindFirst("userId");

            if (userIdClaim == null)
            {
                throw new InvalidOperationException("UserId claim not found in JWT token.");
            }

            return userIdClaim.Value;
        }
    }
}
