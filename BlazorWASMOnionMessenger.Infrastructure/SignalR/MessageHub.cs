using BlazorWASMOnionMessenger.Application.Interfaces.Message;
using BlazorWASMOnionMessenger.Application.Interfaces.Participant;
using BlazorWASMOnionMessenger.Application.Interfaces.SignalR;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BlazorWASMOnionMessenger.Infrastructure.SignalR
{
    [Authorize]
    public class MessageHub : Hub<IMessageClient>, IMessageHub
    {
        private readonly IMessageService messageService;
        private readonly IParticipantService participantService;

        public MessageHub(IMessageService messageService, IParticipantService participantService)
        {
            this.messageService = messageService;
            this.participantService = participantService;
        }

        public async Task SendMessageToChat(CreateMessageDto messageDto)
        {
            var messageId = await messageService.CreateMessageAsync(messageDto);
            var message = await messageService.GetMessageAsync(messageId);
            var participant = await participantService.GetByChatIdAsync(messageDto.ChatId);
            var participantIds = participant.Select(participant => participant.UserId);
            await Clients.Users(participantIds).ReceiveMessage(message);
        }

        public async Task DeleteMessageFromChat(MessageDto messageDto)
        {
            await messageService.DeleteMessageAsync(messageDto.Id);
            var participant = await participantService.GetByChatIdAsync(messageDto.ChatId);
            var participantIds = participant.Select(participant => participant.UserId);
            await Clients.Users(participantIds).DeleteMessage(messageDto);
        }

        public async Task UpdateMessageInChat(MessageDto messageDto)
        {
            await messageService.UpdateMessage(messageDto);
            var participant = await participantService.GetByChatIdAsync(messageDto.ChatId);
            var participantIds = participant.Select(participant => participant.UserId);
            await Clients.Users(participantIds).UpdateMessage(messageDto);
        }

        
    }
}
