using Microsoft.JSInterop;

namespace BlazorWASMOnionMessenger.Client.WebRtc
{
    public interface IWebRtcService
    {
        void CreateAsync(string token);
        Task StartConnection();
        Task Initialize(int chatId);
        Task<IJSObjectReference> StartLocalStream();
        Task Call();
        Task Hangup();
        void SubscribeToSignalWebRtc(Action<int, string, string> handler);
        void UnsubscribeFromSignalWebRtc(Action<int, string, string> handler);
        void SubscribeToOnRemoteStreamAcquired(EventHandler<IJSObjectReference> handler);
    }
}
