using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Participant;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.DTOs.Participant;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ParticipantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddParticipantToChat(CreateParticipantDto createParticipantDto)
        {
            try
            {
                var newParticipant = mapper.Map<Participant>(createParticipantDto);
                unitOfWork.Repository<Participant>().Add(newParticipant);
                await unitOfWork.SaveAsync();
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error occurred while adding a participant to the chat.", ex);
            }
        }

        public async Task<IEnumerable<ParticipantDto>> GetByChatIdAsync(int chatId)
        {
            try
            {
                return await unitOfWork.Repository<Participant>()
                    .GetQueryable(p => p.ChatId == chatId)
                    .ProjectTo<ParticipantDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException($"Error occurred while retrieving participants for chat with ID {chatId}.", ex);
            }
        }
    }
}
