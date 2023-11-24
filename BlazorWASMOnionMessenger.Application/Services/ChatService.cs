using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Chats;
using BlazorWASMOnionMessenger.Application.Interfaces.Common;
using BlazorWASMOnionMessenger.Application.Interfaces.Participant;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class ChatService : IChatService
    {
        private const int PrivateChatTypeId = 1;
        private const int AdminUserId = 1;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISearchPredicateBuilder searchPredicateBuilder;
        private readonly IMapper mapper;
        private readonly IParticipantService participantService;

        public ChatService(IUnitOfWork unitOfWork, ISearchPredicateBuilder searchPredicateBuilder, IMapper mapper, IParticipantService participantService)
        {
            this.unitOfWork = unitOfWork;
            this.searchPredicateBuilder = searchPredicateBuilder;
            this.mapper = mapper;
            this.participantService = participantService;
        }

        public async Task<int> CreateChat(CreateChatDto createChatDto)
        {
            try
            {
                if (createChatDto.ChatTypeId == PrivateChatTypeId)
                {
                    var chatId = await unitOfWork.Repository<Chat>()
                        .GetQueryable(chat =>
                            chat.ChatTypeId == PrivateChatTypeId &&
                            chat.Participants.Any(participant => participant.UserId == createChatDto.ParticipantId)
                        )
                        .Select(chat => chat.Id)
                        .FirstOrDefaultAsync();

                    if (chatId != 0) return chatId;
                }

                var newChat = mapper.Map<Chat>(createChatDto);
                unitOfWork.Repository<Chat>().Add(newChat);
                await unitOfWork.SaveAsync();

                await participantService.AddParticipantToChat(new Domain.DTOs.Participant.CreateParticipantDto
                {
                    ChatId = newChat.Id,
                    UserId = createChatDto.CreatorId,
                    RoleId = AdminUserId
                });

                if (createChatDto.ChatTypeId == PrivateChatTypeId)
                {
                    await participantService.AddParticipantToChat(new Domain.DTOs.Participant.CreateParticipantDto
                    {
                        ChatId = newChat.Id,
                        UserId = createChatDto.ParticipantId
                    });
                }

                return newChat.Id;
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error occurred while creating a chat.", ex);
            }
        }

        public async Task<ChatDto> GetChatById(int chatId)
        {
            try
            {
                var result = await unitOfWork.Repository<Chat>()
                    .GetQueryable(c => c.Id == chatId)
                    .ProjectTo<ChatDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException($"Error occurred while retrieving chat with ID {chatId}.", ex);
            }
        }

        public async Task<PagedEntities<ChatDto>> GetChatsPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            try
            {
                if (page <= 0)
                {
                    throw new ArgumentException("Page number must be greater than 0.", nameof(page));
                }

                if (pageSize <= 0)
                {
                    throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));
                }

                var chatRepository = unitOfWork.Repository<Chat>();
                var searchPredicate = searchPredicateBuilder.BuildSearchPredicate<Chat, ChatDto>(search);
                var query = chatRepository.GetQueryable(searchPredicate);

                var quantity = await query.CountAsync();

                if (!string.IsNullOrEmpty(orderBy)) query = query.OrderBy(orderBy + " " + (orderType ? "desc" : "asc"));

                var pageChat = await query.Skip((page - 1) * pageSize).Take(pageSize)
                    .ProjectTo<ChatDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return new PagedEntities<ChatDto>(pageChat)
                {
                    Quantity = quantity,
                    Pages = (int)Math.Ceiling((double)quantity / pageSize)
                };
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error occurred while retrieving chats.", ex);
            }
        }
    }
}
