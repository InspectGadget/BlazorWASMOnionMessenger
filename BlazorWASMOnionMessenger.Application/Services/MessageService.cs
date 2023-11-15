using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Interfaces.Message;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        public async Task<int> CreateMessageAsync(NewMessageDto newMessage)
        {
            var message = mapper.Map<Message>(newMessage);
            message.CreatedAt = DateTime.Now;
            unitOfWork.Repository<Message>().Add(message);
            await unitOfWork.Save();
            return message.Id;
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(string userId, int chatId, int quantity, int skip)
        {
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
                await unitOfWork.Save();
            }

            return messages;
        }
        public async Task<MessageDto> GetMessageAsync(int messageId)
        {
            return await unitOfWork.Repository<Message>()
                .GetQueryable(msg => msg.Id == messageId)
                .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var message = await unitOfWork.Repository<Message>().GetByIdAsync(messageId);
            if (message != null)
            {
                unitOfWork.Repository<Message>().Delete(message);
                await unitOfWork.Save();
            }
        }

        public async Task UpdateMessage(MessageDto messageDto)
        {
            var message = await unitOfWork.Repository<Message>().GetByIdAsync(messageDto.Id);
            message.MessageText = messageDto.MessageText;
            message.AttachmentUrl = messageDto.AttachmentUrl;
            await unitOfWork.Save();
        }
    }
}
