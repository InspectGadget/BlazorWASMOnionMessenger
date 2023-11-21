using BlazorWASMOnionMessenger.Application.Interfaces.Participant;
using BlazorWASMOnionMessenger.Application.Interfaces.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BlazorWASMOnionMessenger.Infrastructure.SignalR
{
    [Authorize]
    public class WebRtcHub : Hub<IWebRtcClient>, IWebRtcHub
    {
        private readonly IParticipantService participantService;

        public WebRtcHub(IParticipantService participantService)
        {
            this.participantService = participantService;
        }
        public async Task SignalWebRtc(int chatId, string type, string payload)
        {
            var currentUser = Context.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var participant = await participantService.GetByChatIdAsync(chatId);

            var participantIds = participant
                .Where(part => part.UserId != userId)
                .Select(part => part.UserId)
                .ToList();

            await Clients.Users(participantIds).SignalWebRtc(chatId, type, payload);
        }
    }
}
