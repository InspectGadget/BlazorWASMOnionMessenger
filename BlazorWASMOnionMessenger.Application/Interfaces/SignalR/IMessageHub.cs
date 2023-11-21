using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Application.Interfaces.SignalR
{
    public interface IMessageHub
    {
        Task SendMessageToChat(CreateMessageDto messageDto);
        Task DeleteMessageFromChat(MessageDto messageDto);
        Task UpdateMessageInChat(MessageDto messageDto);
    }
}
