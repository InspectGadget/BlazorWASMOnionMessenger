using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using BlazorWASMOnionMessenger.Domain.Common;
using System.Net;

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

        public async Task<TResponse> GetAsync<TResponse>(string requestUri)
        {
            var response = await _client.GetAsync(requestUri); 
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, _options);

            return result;

        }

        public async Task<ResponseDto> PutAsync<TRequest>(string requestUri, TRequest requestDto)
        {
            var content = JsonSerializer.Serialize(requestDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(requestUri, bodyContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Forbidden) return new ResponseDto { ErrorMessage = "Forbidden" };

            var result = JsonSerializer.Deserialize<ResponseDto>(responseContent, _options);
            return result;

        }

        public void SetAuthorizationHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public void RemoveAuthorizationHeader()
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
