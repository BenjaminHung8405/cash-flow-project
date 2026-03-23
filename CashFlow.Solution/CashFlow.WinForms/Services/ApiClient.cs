using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CashFlow.WinForms.Config;

namespace CashFlow.WinForms.Services
{
    /// <summary>
    /// HTTP Client service để gọi các API endpoints từ CashFlow.API
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private string? _authToken;

        public ApiClient()
        {
            _apiBaseUrl = AppSettings.GetApiBaseUrl();
            _httpClient = CreateHttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(AppSettings.GetApiTimeout());
        }

        /// <summary>
        /// Constructor cho phép override URL (dùng cho testing)
        /// </summary>
        public ApiClient(string apiBaseUrl)
        {
            _apiBaseUrl = apiBaseUrl;
            _httpClient = CreateHttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(AppSettings.GetApiTimeout());
        }

        /// <summary>
        /// Tạo HttpClient với SSL bypass cho Development
        /// </summary>
        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

            // Bypass SSL certificate validation cho Development (KHÔNG dùng cho Production!)
            if (IsDevEnvironment())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            }

            return new HttpClient(handler);
        }

        /// <summary>
        /// Kiểm tra xem có phải Development environment không
        /// </summary>
        private static bool IsDevEnvironment()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            return env.Equals("Development", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Đặt token JWT sau khi đăng nhập thành công
        /// </summary>
        public void SetAuthToken(string token)
        {
            _authToken = token;
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Gửi request POST đến API endpoint
        /// </summary>
        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/{endpoint}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"API Error: {response.StatusCode} - {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result ?? throw new InvalidOperationException("Failed to deserialize API response");
            }
            catch (Exception ex)
            {
                throw new Exception($"API request failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gửi request GET đến API endpoint
        /// </summary>
        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/{endpoint}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"API Error: {response.StatusCode} - {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result ?? throw new InvalidOperationException("Failed to deserialize API response");
            }
            catch (Exception ex)
            {
                throw new Exception($"API request failed: {ex.Message}", ex);
            }
        }
    }
}
