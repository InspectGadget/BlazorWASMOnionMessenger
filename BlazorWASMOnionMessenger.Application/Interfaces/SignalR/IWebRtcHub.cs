namespace BlazorWASMOnionMessenger.Application.Interfaces.SignalR
{
    public interface IWebRtcHub
    {
        Task SignalWebRtc(int chatId, string type, string payload);
    }
}
