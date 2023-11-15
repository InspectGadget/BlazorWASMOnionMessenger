using BlazorWASMOnionMessenger.Client.Features.Helpers;
using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;

namespace BlazorWASMOnionMessenger.Client.Features.Chats
{
    public class ChatService : IChatService
    {
        private readonly IHttpClientService httpClientService;

        public ChatService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<int> CreateChat(CreateChatDto createChatDto)
        {
            return await httpClientService.PostAsync<CreateChatDto, int>("chat", createChatDto);
        }

        public async Task<ChatDto> GetChat(int chatId)
        {
            return await httpClientService.GetAsync<ChatDto>($"chat/{chatId}");
        }

        public async Task<PagedEntities<ChatDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            string queryString = QueryStringGenerator.GenerateGridQueryString(page, pageSize, orderBy, orderType, search);
            return await httpClientService.GetAsync<PagedEntities<ChatDto>>($"chat/page?{queryString}");
        }
    }
}
