using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Client.Features.Messages
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> Get(string userId, int chatId, int quantity, int skip);
    }
}
