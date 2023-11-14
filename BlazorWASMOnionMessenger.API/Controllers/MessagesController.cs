using BlazorWASMOnionMessenger.Application.Interfaces.Message;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMOnionMessenger.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessagesController(IMessageService messageService)
        {
            this.messageService = messageService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(string userId, int chatId, int quantity, int skip)
        {
            var messages = await messageService.GetMessagesAsync(userId, chatId, quantity, skip);
            return Ok(messages);
        }
    }
}
