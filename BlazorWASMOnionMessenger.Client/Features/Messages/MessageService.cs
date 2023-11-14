using BlazorWASMOnionMessenger.Client.Features.Helpers;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Client.Features.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IHttpClientService httpClientService;

        public MessageService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }
        public async Task<IEnumerable<MessageDto>> Get(string userId, int chatId, int quantity, int skip)
        {
            string queryString = QueryStringGenerator.GenerateMessageQueryString(userId, chatId, quantity, skip);
            return await httpClientService.GetAsync<IEnumerable<MessageDto>>($"messages?{queryString}");
        }
    }
}
