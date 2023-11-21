using BlazorWASMOnionMessenger.Client.WebRtc;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWASMOnionMessenger.Client.Pages.Chat
{
    public partial class VideoCallDialog
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        private IWebRtcService RtcService { get; set; } = null!;
        private IJSObjectReference? module;
        [Parameter] public int ChatId { get; set; }

        private async void RtcOnOnRemoteStreamAcquired(object? _, IJSObjectReference e)
        {
            if (module == null) throw new InvalidOperationException();
            await module.InvokeVoidAsync("setRemoteStream", e);
            StateHasChanged();
        }

        private async Task HangupAction()
        {
            await RtcService.Hangup();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RtcService.Initialize(ChatId);
                try
                {
                    module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                        "import", "./Pages/Chat/VideoCallDialog.razor.js");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to import JavaScript module: {ex.Message}");
                }
                var stream = await RtcService.StartLocalStream();
                await module.InvokeVoidAsync("setLocalStream", stream);
                RtcService.OnRemoteStreamAcquired += RtcOnOnRemoteStreamAcquired;
                await RtcService.Call();
            }

        }
    }
}
