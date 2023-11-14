using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Application.Interfaces.SignalR
{
    public interface IMessageHub
    {
        Task SendMessageToChat(NewMessageDto messageDto);
    }
}
