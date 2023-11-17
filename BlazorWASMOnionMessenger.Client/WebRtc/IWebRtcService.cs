using Microsoft.JSInterop;

namespace BlazorWASMOnionMessenger.Client.WebRtc
{
    public interface IWebRtcService
    {
        event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;

        Task Join(int chatId);
        Task<IJSObjectReference> StartLocalStream();
        Task Call();
        Task Hangup();
    }
}
