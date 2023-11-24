using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.ChatType;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.Entities;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class ChatTypeService : IChatTypeService
    {
        private readonly IUnitOfWork unitOfWork;

        public ChatTypeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ChatType>> GetChatTypesAsync()
        {
            try
            {
                return await unitOfWork.Repository<ChatType>().GetAllAsync();

            }catch(RepositoryException ex)
            {
                throw new ServiceException($"Error occurred while retrieving chatTypes.", ex);
            }
        }
    }
}
