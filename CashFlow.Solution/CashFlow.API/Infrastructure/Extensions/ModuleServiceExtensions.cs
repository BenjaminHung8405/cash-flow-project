using CashFlow.API.Modules.Auth.Interfaces;
using CashFlow.API.Modules.Auth.Services;
using CashFlow.API.Modules.Funds.Interfaces;
using CashFlow.API.Modules.Funds.Services;

namespace CashFlow.API.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods để đăng ký các Module services
    /// </summary>
    public static class ModuleServiceExtensions
    {
        /// <summary>
        /// Đăng ký tất cả module services
        /// </summary>
        public static IServiceCollection AddModuleServices(this IServiceCollection services)
        {
            // Auth Module Services
            services.AddScoped<IAuthService, AuthService>();

            // Funds Module Services
            services.AddScoped<IFundService, FundService>();

            // TODO: Thêm các module khác
            // Users, Roles, Transactions, v.v.

            return services;
        }
    }
}
