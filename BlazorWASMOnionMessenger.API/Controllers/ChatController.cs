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
        public async Task<ActionResult<PagedEntities<ChatDto>>> GetChatsPage(int page, int pageSize, bool orderType, string orderBy = "", string search = "")
        {
            try
            {
                var pageResult = await chatService.GetChatsPage(page, pageSize, orderBy, orderType, search);
                pageResult.IsSuccessful = true;
                return Ok(pageResult);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new PagedEntities<ChatDto>(new List<ChatDto>()) { ErrorMessage = ex.Message });
            }
        }
        [HttpGet("{chatId}")]
        public async Task<ActionResult<ChatDto>> GetChat(int chatId)
        {
            try
            {
                var result = await chatService.GetChatById(chatId);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateChat(CreateChatDto createChatDto)
        {
            try
            {
                var chatId = await chatService.CreateChat(createChatDto);
                return Ok(chatId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
