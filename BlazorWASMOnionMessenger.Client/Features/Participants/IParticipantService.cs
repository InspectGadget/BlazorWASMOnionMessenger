using BlazorWASMOnionMessenger.Domain.DTOs.Participant;

namespace BlazorWASMOnionMessenger.Client.Features.Participants
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantDto>> GetParticipants();
        Task CreateParticipant(CreateParticipantDto createParticipantDto);
    }
}
