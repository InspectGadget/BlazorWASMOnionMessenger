using BlazorWASMOnionMessenger.Client.Features.Helpers;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using BlazorWASMOnionMessenger.Domain.DTOs.User;

namespace BlazorWASMOnionMessenger.Client.Features.Chats
{
    public class ChatService : IChatService
    {
        private readonly IHttpClientService httpClientService;

        public ChatService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }
        public async Task<PagedEntities<ChatDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            string queryString = QueryStringGenerator.GenerateQueryString(page, pageSize, orderBy, orderType, search);
            return await httpClientService.GetAsync<PagedEntities<ChatDto>>($"chat/page?{queryString}");
        }
    }
}
