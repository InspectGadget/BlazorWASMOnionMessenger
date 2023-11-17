using BlazorWASMOnionMessenger.Client.WebRtc;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWASMOnionMessenger.Client.Pages
{
    public partial class Rtc
    {
        [Inject]
        private IJSRuntime Js { get; set; } = null!;
        [Inject]
        private IWebRtcService RtcService { get; set; } = null!;
        private IJSObjectReference? module;
        private bool startDisabled;
        private bool callDisabled = true;
        private bool hangupDisabled = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await Js.InvokeAsync<IJSObjectReference>(
                    "import", "./Pages/Rtc.razor.js");
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task StartAction()
        {
            if (chatId<=0) return;
            if (module == null) throw new InvalidOperationException();
            if (startDisabled) return;
            startDisabled = true;
            await RtcService.Join(chatId);
            var stream = await RtcService.StartLocalStream();
            await module.InvokeVoidAsync("setLocalStream", stream);
            RtcService.OnRemoteStreamAcquired += RtcOnOnRemoteStreamAcquired;
            callDisabled = false;
        }

        private async void RtcOnOnRemoteStreamAcquired(object? _, IJSObjectReference e)
        {
            if (module == null) throw new InvalidOperationException();
            await module.InvokeVoidAsync("setRemoteStream", e);
            callDisabled = true;
            hangupDisabled = false;
            startDisabled = true;
            StateHasChanged();
        }

        private async Task CallAction()
        {
            if (callDisabled) return;
            callDisabled = true;
            await RtcService.Call();
            hangupDisabled = false;
        }
        private async Task HangupAction()
        {
            await RtcService.Hangup();
            callDisabled = true;
            hangupDisabled = true;
            startDisabled = false;
        }

        private int chatId = 1;
    }
}
