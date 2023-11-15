using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using BlazorWASMOnionMessenger.Domain.Common;
using System.Net;

namespace BlazorWASMOnionMessenger.Client.HttpServices
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;

        public HttpClientService(HttpClient client)
        {
            httpClient = client;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest requestDto)
        {
            var content = JsonSerializer.Serialize(requestDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(requestUri, bodyContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, options);

            return result;
        }

        public async Task<TResponse> GetAsync<TResponse>(string requestUri)
        {
            var response = await httpClient.GetAsync(requestUri); 
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, options);

            return result;

        }

        public async Task<ResponseDto> PutAsync<TRequest>(string requestUri, TRequest requestDto)
        {
            var content = JsonSerializer.Serialize(requestDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(requestUri, bodyContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Forbidden) return new ResponseDto { ErrorMessage = "Forbidden" };

            var result = JsonSerializer.Deserialize<ResponseDto>(responseContent, options);
            return result;

        }

        public void SetAuthorizationHeader(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public void RemoveAuthorizationHeader()
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
