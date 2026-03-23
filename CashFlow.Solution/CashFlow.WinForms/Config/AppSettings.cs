using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CashFlow.WinForms.Config
{
    /// <summary>
    /// Lớp để quản lý cấu hình ứng dụng từ appsettings.json
    /// </summary>
    public static class AppSettings
    {
        private static IConfiguration _configuration;

        static AppSettings()
        {
            _configuration = LoadConfiguration();
        }

        /// <summary>
        /// Tải cấu hình từ appsettings.json
        /// </summary>
        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{GetEnvironment()}.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        /// <summary>
        /// Lấy environment hiện tại (Development/Production)
        /// </summary>
        private static string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        }

        /// <summary>
        /// Lấy URL Backend API
        /// </summary>
        public static string GetApiBaseUrl()
        {
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            return baseUrl ?? throw new InvalidOperationException("ApiSettings:BaseUrl not configured");
        }

        /// <summary>
        /// Lấy Timeout cho HTTP requests (tính bằng giây)
        /// </summary>
        public static int GetApiTimeout()
        {
            var timeout = _configuration["ApiSettings:Timeout"];
            return int.TryParse(timeout, out var result) ? result : 30;
        }

        /// <summary>
        /// Reload cấu hình (hữu ích khi file thay đổi)
        /// </summary>
        public static void Reload()
        {
            _configuration = LoadConfiguration();
        }
    }
}
