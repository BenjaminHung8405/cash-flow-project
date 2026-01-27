namespace CashFlow.API.Infrastructure.Services
{
    /// <summary>
    /// Interface for tenant resolution services.
    /// Provides methods to get the current tenant ID from the HTTP context.
    /// </summary>
    public interface ITenantService
    {
        /// <summary>
        /// Gets the current tenant ID from the JWT token claims.
        /// </summary>
        /// <returns>The current tenant ID</returns>
        Guid GetCurrentTenantId();

        /// <summary>
        /// Gets the current user ID from the JWT token claims.
        /// </summary>
        /// <returns>The current user ID</returns>
        string GetCurrentUserId();
    }
}
