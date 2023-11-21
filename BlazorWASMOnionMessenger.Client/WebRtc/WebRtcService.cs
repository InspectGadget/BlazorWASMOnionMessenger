using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Radzen;

namespace BlazorWASMOnionMessenger.Client.WebRtc
{
    public class WebRtcService : IWebRtcService
    {
        private readonly DialogService dialogService;
        private readonly IJSRuntime js;
        private IJSObjectReference? jsModule;
        private HubConnection hubConnection;
        private readonly string hubUrl;
        private int? chatId;
        private event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;
        private event Action<int, string, string> OnSignalWebRtc;


        public WebRtcService(IJSRuntime js, DialogService dialogService, string hubUrl)
        {
            this.js = js;
            this.dialogService = dialogService;
            this.hubUrl = hubUrl;
        }
        public void CreateAsync(string token)
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .WithAutomaticReconnect()
            .Build();

            hubConnection.On<int, string, string>("SignalWebRtc", (chatId, type, payload) =>
            {
                OnSignalWebRtc?.Invoke(chatId, type, payload);
            });
        }
        public async Task StartConnection()
        {
            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("SignalR connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while connecting to SignalR hub: {ex.Message}");
            }
        }
        public async Task Initialize(int chatId)
        {
            this.chatId = chatId;
            SubscribeToSignalWebRtc(HandleSignalWebRtc);
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
            UnsubscribeFromSignalWebRtc(HandleSignalWebRtc);
            dialogService.Close();

            chatId = null;
        }
        public void SubscribeToSignalWebRtc(Action<int, string, string> handler)
        {
            OnSignalWebRtc += handler;
        }

        public void UnsubscribeFromSignalWebRtc(Action<int, string, string> handler)
        {
            OnSignalWebRtc -= handler;
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
            await hubConnection.SendAsync("SignalWebRtc", chatId, "offer", offer);
        }

        [JSInvokable]
        public async Task SendAnswer(string answer)
        {
            await hubConnection.SendAsync("SignalWebRtc", chatId, "answer", answer);
        }

        [JSInvokable]
        public async Task SendCandidate(string candidate)
        {
            await hubConnection.SendAsync("SignalWebRtc", chatId, "candidate", candidate);
        }

        [JSInvokable]
        public async Task SetRemoteStream()
        {
            if (jsModule == null) throw new InvalidOperationException();
            var stream = await jsModule.InvokeAsync<IJSObjectReference>("getRemoteStream");
            OnRemoteStreamAcquired?.Invoke(this, stream);
        }

        public void SubscribeToOnRemoteStreamAcquired(EventHandler<IJSObjectReference> handler)
        {
            OnRemoteStreamAcquired += handler;
        }
    }
}
