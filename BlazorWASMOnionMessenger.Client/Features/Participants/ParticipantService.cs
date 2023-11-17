using BlazorWASMOnionMessenger.Client.HttpServices;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Participant;

namespace BlazorWASMOnionMessenger.Client.Features.Participants
{
    public class ParticipantService : IParticipantService
    {
        private readonly IHttpClientService httpClientService;

        public ParticipantService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }
        public async Task CreateParticipant(CreateParticipantDto createParticipantDto)
        {
            await httpClientService.PostAsync<CreateParticipantDto, ResponseDto>("participant", createParticipantDto);
        }

        public async Task<IEnumerable<ParticipantDto>> GetParticipants()
        {
            return await httpClientService.GetAsync<IEnumerable<ParticipantDto>>("participant");
        }
    }
}
