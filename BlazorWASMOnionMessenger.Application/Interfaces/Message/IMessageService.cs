using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Message
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetMessagesAsync(string userId, int chatId, int quantity, int skip);
        Task<int> CreateMessageAsync(NewMessageDto newMessage);
        Task<MessageDto> GetMessageAsync(int messageId);
        Task DeleteMessageAsync(int messageId);
        Task UpdateMessage(MessageDto messageDto);
    }
}
