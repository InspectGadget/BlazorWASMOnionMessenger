namespace BlazorWASMOnionMessenger.Application.Interfaces.SignalR
{
    public interface IWebRtcClient
    {
        Task SignalWebRtc(int chatId, string type, string payload);
    }
}
