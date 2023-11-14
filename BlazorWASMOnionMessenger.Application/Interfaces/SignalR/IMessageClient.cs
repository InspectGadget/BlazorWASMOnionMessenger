using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Application.Interfaces.SignalR
{
    public interface IMessageClient
    {
        Task ReceiveMessage(MessageDto messageDto);
    }
}
