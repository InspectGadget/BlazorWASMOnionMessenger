using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Client.HttpServices
{
    public interface IHttpClientService
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest requestDto);
        Task<TResponse> GetAsync<TResponse>(string requestUri);
        Task<ResponseDto> PutAsync<TRequest>(string requestUri, TRequest requestDto);
        void SetAuthorizationHeader(string token);
        void RemoveAuthorizationHeader();
    }
}
