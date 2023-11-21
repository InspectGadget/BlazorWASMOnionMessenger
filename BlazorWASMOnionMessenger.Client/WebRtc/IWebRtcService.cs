using Microsoft.JSInterop;

namespace BlazorWASMOnionMessenger.Client.WebRtc
{
    public interface IWebRtcService
    {
        event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;

        Task Initialize(int chatId);
        Task<IJSObjectReference> StartLocalStream();
        Task Call();
        Task Hangup();
    }
}
