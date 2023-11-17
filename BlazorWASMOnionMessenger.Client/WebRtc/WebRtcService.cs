using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace BlazorWASMOnionMessenger.Client.WebRtc
{
    public class WebRtcService : IWebRtcService
    {
        private readonly NavigationManager navigationMenager;
        private readonly IJSRuntime js;
        private IJSObjectReference? jsModule;
        private DotNetObjectReference<WebRtcService>? jsThis;
        private HubConnection? hub;
        private int? chatId;
        public event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;

        public WebRtcService(IJSRuntime js, NavigationManager nav)
        {
            this.js = js;
            navigationMenager = nav;
        }

        public async Task Join(int chatId)
        {
            if (this.chatId != null) throw new InvalidOperationException();
            this.chatId = chatId;
            var hub = await GetHub();
            await hub.SendAsync("join", chatId);
            jsModule = await js.InvokeAsync<IJSObjectReference>(
                "import", "/js/WebRtcService.cs.js");
            jsThis = DotNetObjectReference.Create(this);
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

        public async Task Hangup()
        {
            if (jsModule == null) throw new InvalidOperationException();
            await jsModule.InvokeVoidAsync("hangupAction");

            var hub = await GetHub();
            await hub.SendAsync("leave", chatId);

            chatId = null;
        }

        private async Task<HubConnection> GetHub()
        {

            if (this.hub != null) return this.hub;

            var hub = new HubConnectionBuilder()
                .WithUrl(navigationMenager.ToAbsoluteUri("/messagehub"))
                .Build();

            hub.On<int, string, string>("SignalWebRtc", async (chatId, type, payload) =>
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
            });

            await hub.StartAsync();
            this.hub = hub;
            return this.hub;
        }

        [JSInvokable]
        public async Task SendOffer(string offer)
        {
            var hub = await GetHub();
            await hub.SendAsync("SignalWebRtc", chatId, "offer", offer);
        }

        [JSInvokable]
        public async Task SendAnswer(string answer)
        {
            var hub = await GetHub();
            await hub.SendAsync("SignalWebRtc", chatId, "answer", answer);
        }

        [JSInvokable]
        public async Task SendCandidate(string candidate)
        {
            var hub = await GetHub();
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
