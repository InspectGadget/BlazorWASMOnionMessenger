using BlazorWASMOnionMessenger.Client.Features.Messages;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorWASMOnionMessenger.Client.Pages.Chat
{
    public partial class Chat : IDisposable
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        private ISignalRMessageService signalRMessageService { get; set; }
        [Inject]
        private IMessageService messageService { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public string ChatId { get; set; }
        protected List<MessageDto> Messages { get; set; } = new List<MessageDto>();
        protected NewMessageDto NewMessage { get; set; } = new NewMessageDto();
        protected string userId { get; set; }
        private int skip = 0;
        private int quantity = 30;
        protected string MessageContainerClass(string senderId) =>
        senderId == userId ? "float-right" : "";
        protected ElementReference messagesContainerRef;

        protected override async Task OnInitializedAsync()
        {
            signalRMessageService.SubscribeToMessageReceived(receiveMessage);
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            userId = user.FindFirst("nameid").Value;
            Messages = (await messageService.Get(userId, int.Parse(ChatId), quantity, skip)).ToList();
        }

        private void receiveMessage(MessageDto message)
        {
            Messages.Add(message);
            StateHasChanged();
        }
        protected async Task sendMessage()
        {
            NewMessage.ChatId = int.Parse(ChatId); 
            NewMessage.UserId = userId;
            NewMessage.AttachmentUrl = "tmp";
            await signalRMessageService.SendMessageToChat(NewMessage);
            NewMessage.MessageText = "";
            StateHasChanged();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("scrollToBottom", messagesContainerRef);
        }

        public void Dispose()
        {
            signalRMessageService.UnsubscribeFromMessageReceived(receiveMessage);
        }
    }
}
