using BlazorWASMOnionMessenger.Application.Interfaces.Participant;
using BlazorWASMOnionMessenger.Domain.DTOs.Participant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMOnionMessenger.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService participantService;

        public ParticipantController(IParticipantService participantService)
        {
            this.participantService = participantService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetByChatId(int chatId)
        {
            try
            {
                var result = participantService.GetByChatIdAsync(chatId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateParticipant(CreateParticipantDto createParticipantDto)
        {
            try
            {
                await participantService.AddParticipantToChat(createParticipantDto);
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
