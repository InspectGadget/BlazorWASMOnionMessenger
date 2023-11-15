using BlazorWASMOnionMessenger.Client.Features.Common;
using BlazorWASMOnionMessenger.Client.Features.Messages;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using System;

namespace BlazorWASMOnionMessenger.Client.Pages.Chat
{
    public partial class Chat : IDisposable
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }
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
        protected bool isUpdating = false;
        protected MessageDto messageToUpdate = new MessageDto();

        protected override async Task OnInitializedAsync()
        {
            signalRMessageService.SubscribeToReceiveMessage(HendleReceiveMessage);
            signalRMessageService.SubscribeToUpdateMessage(HandleUpdateMessage);
            signalRMessageService.SubscribeToDeleteMessage(HandleDeleteMessage);
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            userId = user.FindFirst("nameid").Value;
            Messages = (await messageService.Get(userId, int.Parse(ChatId), quantity, skip)).ToList();
        }

        private void HendleReceiveMessage(MessageDto message)
        {
            Messages.Add(message);
            StateHasChanged();
        }
        protected async Task SendMessage()
        {
            NewMessage.ChatId = int.Parse(ChatId); 
            NewMessage.UserId = userId;
            NewMessage.AttachmentUrl = "tmp";
            await signalRMessageService.SendMessageToChat(NewMessage);
            NewMessage.MessageText = "";
            StateHasChanged();
        }
        protected async Task UpdateMessage()
        {
            messageToUpdate.MessageText = NewMessage.MessageText;
            await signalRMessageService.UpdateMessageInChat(messageToUpdate);
            NewMessage.MessageText = "";
            isUpdating = false;
            StateHasChanged();
        }
        private void HandleUpdateMessage(MessageDto messageDto)
        {
            if (int.Parse(ChatId) == messageDto.ChatId)
            {
                int index = Messages.FindIndex(m => m.Id == messageDto.Id);

                if (index != -1)
                {
                    Messages[index] = messageDto;
                }
            }
            StateHasChanged();
        }
        protected async Task DeleteMessage(int messageId)
        {
            await signalRMessageService.DeleteMessageFromChat(Messages.First(m => m.Id == messageId));
        }
        private void HandleDeleteMessage(MessageDto messageDto)
        {
            if (int.Parse(ChatId) == messageDto.ChatId)
            {
                var messageToRemove = Messages.First(m => m.Id == messageDto.Id);
                Messages.Remove(messageToRemove);
            }
            StateHasChanged();
        }
        void ShowContextMenuWithItems(MouseEventArgs args, MessageDto messageDto)
        {
            ContextMenuService.Open(args,
                messageDto.SenderId == userId ? ContextMenuSets.deleteAndEdit : ContextMenuSets.deleteOnly,
                async (menuArgs) => await OnMenuItemClickAsync(menuArgs, messageDto.Id));
        }
        async Task OnMenuItemClickAsync(MenuItemEventArgs args, int messageId)
        {
            switch (args.Value)
            {
                case "edit":
                    isUpdating = true;
                    messageToUpdate = Messages.First(m => m.Id == messageId);
                    NewMessage.MessageText = Messages.First(m => m.Id == messageId).MessageText;
                    StateHasChanged();
                    break;
                case "delete":
                    await DeleteMessage(messageId);
                    break;
            }
            ContextMenuService.Close();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("scrollToBottom", messagesContainerRef);
        }

        public void Dispose()
        {
            signalRMessageService.UnsubscribeFromUpdateMessage(HendleReceiveMessage);
        }
    }
}
