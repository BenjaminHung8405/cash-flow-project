namespace CashFlow.API
{
    public class ApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public ApiClient()
        {
            if(_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://localhost:7071/api/");
            }
        }
        public HttpClient GetClient()
        {
            // neu trong db co token thi add vao header " Authorization: Bearer {token} "
            if (AppSession.IsAuthenticated)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AppSession.JwtToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
            return _httpClient;
        }
    }
}
