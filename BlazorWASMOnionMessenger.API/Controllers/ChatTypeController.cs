using BlazorWASMOnionMessenger.Application.Interfaces.ChatType;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMOnionMessenger.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chatTypes")]
    public class ChatTypeController : ControllerBase
    {
        private readonly IChatTypeService chatTypeService;

        public ChatTypeController(IChatTypeService chatTypeService)
        {
            this.chatTypeService = chatTypeService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatType>>> GetChatTypes()
        {
            var chatTypes = await chatTypeService.GetChatTypesAsync();
            return Ok(chatTypes);
        }
    }
}
