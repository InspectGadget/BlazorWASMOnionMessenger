using System.Text.Json;
using System.Text;

namespace BlazorWASMOnionMessenger.Client.HttpServices
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public HttpClientService(HttpClient client)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest requestDto)
        {
            var content = JsonSerializer.Serialize(requestDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(requestUri, bodyContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, _options);

            return result;
        }
    }
}
