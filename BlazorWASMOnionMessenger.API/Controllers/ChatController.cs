using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Chats;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMOnionMessenger.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [HttpGet("page")]
        public async Task<ActionResult<PagedEntities<ChatDto>>> GetUsersPage(int page, int pageSize, bool orderType, string orderBy = "", string search = "")
        {
            try
            {
                var pageResult = await chatService.GetPage(page, pageSize, orderBy, orderType, search);
                pageResult.IsSuccessful = true;
                return Ok(pageResult);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new PagedEntities<ChatDto>(new List<ChatDto>()) { ErrorMessage = ex.Message });
            }
        }
    }
}
