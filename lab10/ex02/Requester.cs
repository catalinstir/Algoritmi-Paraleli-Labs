using System.Text.Json;

namespace ex02
{
    public class Requester
    {
        private string _targetUrl;
        private HttpClient _httpClient;

        public Requester(string targetUrl)
        {
            _targetUrl = targetUrl;
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse> GetImageURL()
        {
            string apiResponseString = await _httpClient.GetStringAsync(_targetUrl);

            JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
            serializerOptions.PropertyNameCaseInsensitive = true;

            ApiResponse response = JsonSerializer.Deserialize<ApiResponse>(apiResponseString, serializerOptions);

            return response;
        }
    }
}
