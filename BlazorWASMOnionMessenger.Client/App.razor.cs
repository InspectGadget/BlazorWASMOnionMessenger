using Blazored.LocalStorage;
using BlazorWASMOnionMessenger.Client.Features.Messages;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BlazorWASMOnionMessenger.Client
{
    public partial class App
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        private ISignalRMessageService signalRMessageService { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        private ILocalStorageService localStorage { get; set; }

        private AuthenticationState authenticationState;

        protected override async Task OnInitializedAsync()
        {
            await HandleSignalRConnection();
            AuthenticationStateProvider.AuthenticationStateChanged += HandleAuthenticationStateChanged;
        }

        private void ShowNotification(MessageDto messageDto)
        {
            var userId = authenticationState.User.FindFirst("nameid")?.Value;

            if (userId != null && messageDto.SenderId != userId)
            {
                string messagePreview = string.Concat(": ", messageDto.MessageText.AsSpan(0, Math.Min(20, messageDto.MessageText.Length)));
                NotificationService.Notify(NotificationSeverity.Info, messageDto.ChatName, messageDto.SenderName + messagePreview, 4000);
            }
        }

        private async void HandleAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask) => await HandleSignalRConnection();
        private async Task HandleSignalRConnection()
        {
            authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var token = await localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token)) return;

            signalRMessageService.CreateAsync(token);
            signalRMessageService.SubscribeToReceiveMessage(ShowNotification);
            await signalRMessageService.StartConnection();
        }
    }
}
