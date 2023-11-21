using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWASMOnionMessenger.Client.Features.Messages
{
    public interface ISignalRMessageService
    {
        void CreateAsync(string token);
        Task StartConnection();
        Task SendMessageToChat(CreateMessageDto newMessageDto);
        Task UpdateMessageInChat(MessageDto messageDto);
        Task DeleteMessageFromChat(MessageDto messageDto);
        void SubscribeToReceiveMessage(Action<MessageDto> handler);
        void UnsubscribeFromReceiveMessage(Action<MessageDto> handler);
        void SubscribeToDeleteMessage(Action<MessageDto> handler);
        void UnsubscribeFromDeleteMessage(Action<MessageDto> handler);
        void SubscribeToUpdateMessage(Action<MessageDto> handler);
        void UnsubscribeFromUpdateMessage(Action<MessageDto> handler);
    }
}
