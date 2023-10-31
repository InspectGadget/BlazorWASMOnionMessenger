namespace BlazorWASMOnionMessenger.Client.HttpServices
{
    public interface IHttpClientService
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest requestDto);
    }
}
