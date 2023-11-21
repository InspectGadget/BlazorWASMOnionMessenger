using BlazorWASMOnionMessenger.Domain.DTOs.Message;

namespace BlazorWASMOnionMessenger.Application.Interfaces.SignalR
{
    public interface IMessageClient
    {
        Task ReceiveMessage(MessageDto messageDto);
        Task DeleteMessage(MessageDto messageDto);
        Task UpdateMessage(MessageDto messageDto);
        Task Join(string connectionId);
        Task Leave(string connectionId);
        Task SignalWebRtc(int chatId, string type, string payload);
    }
}
