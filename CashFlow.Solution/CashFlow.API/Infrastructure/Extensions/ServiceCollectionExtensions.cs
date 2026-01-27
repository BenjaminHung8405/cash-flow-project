using CashFlow.API.Data;
using CashFlow.API.Infrastructure.Security;
using CashFlow.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CashFlow.API.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for dependency injection configuration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds multi-tenancy services to the dependency injection container.
        /// </summary>
        public static IServiceCollection AddMultiTenancy(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantService, TenantService>();
            return services;
        }

        /// <summary>
        /// Adds the multi-tenant AppDbContext to the dependency injection container.
        /// </summary>
        public static IServiceCollection AddMultiTenantDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(
                    connectionString,
                    sqlOptions => sqlOptions
                        .MigrationsHistoryTable("__EFMigrationsHistory")
                        .MigrationsAssembly("CashFlow.API")
                );

                // Enable sensitive data logging in development
                if (configuration["Environment"] == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.LogTo(Console.WriteLine);
                }
            });

            return services;
        }

        /// <summary>
        /// Configures JWT authentication with tenant claim validation.
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"];
            var jwtIssuer = configuration["Jwt:Issuer"];
            var jwtAudience = configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key is not configured in appsettings.json");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteAsJsonAsync(new
                                {
                                    message = "Token has expired"
                                });
                            }

                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteAsJsonAsync(new
                                {
                                    message = "Invalid or missing authentication token"
                                });
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
            services.AddScoped<JwtTokenGenerator>();

            return services;
        }

        /// <summary>
        /// Adds configured CORS policies.
        /// </summary>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // Development policy - allow all
                options.AddPolicy("AllowAll", corsBuilder =>
                {
                    corsBuilder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                });

                // Production policy - restricted
                options.AddPolicy("AllowProduction", corsBuilder =>
                {
                    corsBuilder.WithOrigins("https://yourdomain.com")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                });
            });

            return services;
        }
    }
}
