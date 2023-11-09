using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;


namespace BlazorWASMOnionMessenger.Application.Interfaces.Chats
{
    public interface IChatService
    {
        Task<PagedEntities<ChatDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search);
    }
}
