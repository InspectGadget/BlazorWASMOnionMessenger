using BlazorWASMOnionMessenger.Domain.DTOs.Participant;

namespace BlazorWASMOnionMessenger.Client.Features.Participants
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantDto>> GetParticipants(int chatId);
        Task CreateParticipant(CreateParticipantDto createParticipantDto);
    }
}
