using BlazorWASMOnionMessenger.Domain.DTOs.Participant;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Participant
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantDto>> GetByChatIdAsync(int chatId);
        Task AddParticipantToChat(CreateParticipantDto createParticipantDto);
    }
}
