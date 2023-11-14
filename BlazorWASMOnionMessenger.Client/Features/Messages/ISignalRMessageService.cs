using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Client.Features.Messages
{
    public interface ISignalRMessageService
    {
        event Action<MessageDto> OnMessageReceived;
        void CreateAsync(string token);
        Task StartConnection();
        Task SendMessageToChat(NewMessageDto messageDto);
        void SubscribeToMessageReceived(Action<MessageDto> handler);
        void UnsubscribeFromMessageReceived(Action<MessageDto> handler);
    }
}
