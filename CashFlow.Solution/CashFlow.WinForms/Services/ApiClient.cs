using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CashFlow.WinForms.Services
{
    /// <summary>
    /// HTTP Client service để gọi API từ WinForms
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private string _jwtToken;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClient(string apiBaseUrl = "https://localhost:5001")
        {
            _apiBaseUrl = apiBaseUrl;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        /// <summary>
        /// Set JWT Token sau khi login
        /// </summary>
        public void SetJwtToken(string token)
        {
            _jwtToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/auth/login", content);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Login failed: {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseJson, JsonOptions);

            // Lưu token
            SetJwtToken(loginResponse.Token);

            return loginResponse;
        }

        /// <summary>
        /// Lấy danh sách các quỹ
        /// </summary>
        public async Task<List<FundDto>> GetFundsAsync()
        {
            ThrowIfNotAuthenticated();

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/funds");
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Get funds failed: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var funds = JsonSerializer.Deserialize<List<FundDto>>(json, JsonOptions);

            return funds ?? new List<FundDto>();
        }

        /// <summary>
        /// Lấy chi tiết một quỹ
        /// </summary>
        public async Task<FundDto> GetFundByIdAsync(string fundId)
        {
            ThrowIfNotAuthenticated();

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/funds/{fundId}");
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Get fund failed: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var fund = JsonSerializer.Deserialize<FundDto>(json, JsonOptions);

            return fund;
        }

        /// <summary>
        /// Tạo quỹ mới
        /// </summary>
        public async Task<FundDto> CreateFundAsync(CreateFundRequest request)
        {
            ThrowIfNotAuthenticated();

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/funds", content);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Create fund failed: {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var fund = JsonSerializer.Deserialize<FundDto>(responseJson, JsonOptions);

            return fund;
        }

        /// <summary>
        /// Cập nhật quỹ
        /// </summary>
        public async Task<FundDto> UpdateFundAsync(string fundId, UpdateFundRequest request)
        {
            ThrowIfNotAuthenticated();

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/api/funds/{fundId}", content);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Update fund failed: {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            
            // API có thể return full object hoặc success message
            // Adjust parsing based on API response
            var fund = JsonSerializer.Deserialize<FundDto>(responseJson, JsonOptions);

            return fund;
        }

        /// <summary>
        /// Xóa quỹ
        /// </summary>
        public async Task DeleteFundAsync(string fundId)
        {
            ThrowIfNotAuthenticated();

            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/funds/{fundId}");
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Delete fund failed: {response.StatusCode}");
            }
        }

        /// <summary>
        /// Lấy lịch sử thay đổi của quỹ
        /// </summary>
        public async Task<List<AuditLogDto>> GetFundAuditTrailAsync(string fundId)
        {
            ThrowIfNotAuthenticated();

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/funds/{fundId}/audit-trail");
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Get audit trail failed: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var auditLogs = JsonSerializer.Deserialize<List<AuditLogDto>>(json, JsonOptions);

            return auditLogs ?? new List<AuditLogDto>();
        }

        private void ThrowIfNotAuthenticated()
        {
            if (string.IsNullOrEmpty(_jwtToken))
            {
                throw new InvalidOperationException("Not authenticated. Call LoginAsync first.");
            }
        }
    }

    // ============ DTOs ============

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

    public class FundDto
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string FundName { get; set; }
        public string FundType { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateFundRequest
    {
        public string FundName { get; set; }
        public string FundType { get; set; }
        public string AccountNumber { get; set; }
        public decimal InitialBalance { get; set; }
    }

    public class UpdateFundRequest
    {
        public string FundName { get; set; }
        public string FundType { get; set; }
        public string AccountNumber { get; set; }
        public bool? IsActive { get; set; }
    }

    public class AuditLogDto
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
