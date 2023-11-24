using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Message;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<int> CreateMessageAsync(CreateMessageDto newMessage)
        {
            try
            {
                var message = mapper.Map<Message>(newMessage);
                message.CreatedAt = DateTime.Now;
                unitOfWork.Repository<Message>().Add(message);
                await unitOfWork.SaveAsync();
                return message.Id;
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error occurred while creating a message.", ex);
            }
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(string userId, int chatId, int quantity, int skip)
        {
            try
            {
                if (quantity <= 0 || skip < 0)
                {
                    throw new ValidationException("Quantity must be greater than zero and Skip must be greater than or equal to zero.");
                }
                var query = unitOfWork.Repository<Message>()
                    .GetQueryable(
                        filter: msg => msg.ChatId == chatId,
                        orderBy: q => q.OrderBy(msg => msg.CreatedAt)
                    );

                var messages = await query.Skip(skip).Take(quantity)
                    .ProjectTo<MessageDto>(mapper.ConfigurationProvider).ToListAsync();

                var unreadMessagesToDelete = await unitOfWork.Repository<UnreadMessage>()
                    .GetQueryable(filter: um => um.UserId == userId && um.Message.ChatId == chatId).ToListAsync();

                if (unreadMessagesToDelete.Count > 0)
                {
                    unitOfWork.Repository<UnreadMessage>().DeleteRange(unreadMessagesToDelete);
                    await unitOfWork.SaveAsync();
                }

                return messages;
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error occurred while retrieving messages.", ex);
            }
        }

        public async Task<MessageDto?> GetMessageAsync(int messageId)
        {
            try
            {
                return await unitOfWork.Repository<Message>()
                    .GetQueryable(msg => msg.Id == messageId)
                    .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException($"Error occurred while retrieving message with ID {messageId}.", ex);
            }
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            try
            {
                var message = await unitOfWork.Repository<Message>().GetByIdAsync(messageId);
                if (message == null) throw new ServiceException($"Error occurred while deleting, message with ID {messageId} doesn't exist.");
                unitOfWork.Repository<Message>().Delete(message);
                await unitOfWork.SaveAsync();
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException($"Error occurred while deleting message with ID {messageId}.", ex);
            }
        }

        public async Task UpdateMessage(MessageDto messageDto)
        {
            try
            {
                var message = await unitOfWork.Repository<Message>().GetByIdAsync(messageDto.Id);

                if (message == null)
                {
                    throw new ServiceException($"Message with ID {messageDto.Id} not found.");
                }
                message.MessageText = messageDto.MessageText;
                message.AttachmentUrl = messageDto.AttachmentUrl;
                await unitOfWork.SaveAsync();
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException($"Error occurred while updating message with ID {messageDto.Id}.", ex);
            }
        }
    }
}
