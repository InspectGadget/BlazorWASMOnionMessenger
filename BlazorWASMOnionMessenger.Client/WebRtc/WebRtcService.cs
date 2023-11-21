using BlazorWASMOnionMessenger.Client.Features.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Radzen;

namespace BlazorWASMOnionMessenger.Client.WebRtc
{
    public class WebRtcService : IWebRtcService
    {
        private readonly ISignalRMessageService signalRMessageService;
        private readonly DialogService dialogService;
        private readonly IJSRuntime js;
        private IJSObjectReference? jsModule;
        private readonly HubConnection hub;
        private int? chatId;
        public event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;

        public WebRtcService(IJSRuntime js, ISignalRMessageService signalRMessageService, DialogService dialogService)
        {
            this.js = js;
            this.signalRMessageService = signalRMessageService;
            this.dialogService = dialogService;
            hub = this.signalRMessageService.GetHub();
        }

        public async Task Initialize(int chatId)
        {
            this.chatId = chatId;
            signalRMessageService.SubscribeToSignalWebRtc(HandleSignalWebRtc);
            jsModule = await js.InvokeAsync<IJSObjectReference>(
                "import", "/js/WebRtcService.cs.js");
            var jsThis = DotNetObjectReference.Create(this);
            await jsModule.InvokeVoidAsync("initialize", jsThis);
        }
        public async Task<IJSObjectReference> StartLocalStream()
        {
            if (jsModule == null) throw new InvalidOperationException();
            var stream = await jsModule.InvokeAsync<IJSObjectReference>("startLocalStream");
            return stream;
        }
        public async Task Call()
        {
            if (jsModule == null) throw new InvalidOperationException();
            var offerDescription = await jsModule.InvokeAsync<string>("callAction");
            await SendOffer(offerDescription);
        }

        [JSInvokable]
        public async Task Hangup()
        {
            if (jsModule == null) throw new InvalidOperationException();
            await jsModule.InvokeVoidAsync("hangupAction");
            signalRMessageService.UnsubscribeFromSignalWebRtc(HandleSignalWebRtc);
            dialogService.Close();

            chatId = null;
        }

        private async void HandleSignalWebRtc(int chatId, string type, string payload)
        {
            if (jsModule == null) throw new InvalidOperationException();

            if (this.chatId != chatId) return;
            switch (type)
            {
                case "offer":
                    await jsModule.InvokeVoidAsync("processOffer", payload);
                    break;
                case "answer":
                    await jsModule.InvokeVoidAsync("processAnswer", payload);
                    break;
                case "candidate":
                    await jsModule.InvokeVoidAsync("processCandidate", payload);
                    break;
            }
        }

        [JSInvokable]
        public async Task SendOffer(string offer)
        {
            await hub.SendAsync("SignalWebRtc", chatId, "offer", offer);
        }

        [JSInvokable]
        public async Task SendAnswer(string answer)
        {
            await hub.SendAsync("SignalWebRtc", chatId, "answer", answer);
        }

        [JSInvokable]
        public async Task SendCandidate(string candidate)
        {
            await hub.SendAsync("SignalWebRtc", chatId, "candidate", candidate);
        }

        [JSInvokable]
        public async Task SetRemoteStream()
        {
            if (jsModule == null) throw new InvalidOperationException();
            var stream = await jsModule.InvokeAsync<IJSObjectReference>("getRemoteStream");
            OnRemoteStreamAcquired?.Invoke(this, stream);
        }
    }
}
