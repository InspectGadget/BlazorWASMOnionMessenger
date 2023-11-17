using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Interfaces.Chats;
using BlazorWASMOnionMessenger.Application.Interfaces.Common;
using BlazorWASMOnionMessenger.Application.Interfaces.Participant;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
            if (createChatDto.ChatTypeId == PrivateChatTypeId)
            {
                var chatId = unitOfWork.Repository<Chat>()
                    .GetQueryable(chat =>
                        chat.ChatTypeId == PrivateChatTypeId &&
                        chat.Participants.Any(participant => participant.UserId == createChatDto.ParticipantId)
                    )
                    .Select(chat => chat.Id)
                    .FirstOrDefault();
                if (chatId != 0) return chatId;
            }

            var newChat = mapper.Map<Chat>(createChatDto);
            newChat.CreatedAt = DateTime.Now;
            unitOfWork.Repository<Chat>().Add(newChat);
            await unitOfWork.SaveAsync();

            await participantService.AddParticipantToChat(new Domain.DTOs.Participant.CreateParticipantDto
            {
                ChatId = newChat.Id,
                UserId = createChatDto.CreatorId,
                RoleId = AdminUserId
            });

            if(createChatDto.ChatTypeId == PrivateChatTypeId)
            {
                await participantService.AddParticipantToChat(new Domain.DTOs.Participant.CreateParticipantDto
                {
                    ChatId = newChat.Id,
                    UserId = createChatDto.ParticipantId
                });
            }

            return newChat.Id;

        }

        public async Task<ChatDto> GetChatById(int chatId)
        {
            return mapper.Map<ChatDto>(await unitOfWork.Repository<Chat>().GetByIdAsync(chatId));
        }

        public async Task<PagedEntities<ChatDto>> GetChatsPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            var chatRepository = unitOfWork.Repository<Chat>();

            var searchPredicate = searchPredicateBuilder.BuildSearchPredicate<Chat, ChatDto>(search);

            var query = chatRepository.GetQueryable(searchPredicate);

            var quantity = query.Count();

            if (!string.IsNullOrEmpty(orderBy)) query = query.OrderBy(orderBy + " " + (orderType ? "desc" : "asc"));

            var pageChat = await query.Skip((page - 1) * pageSize).Take(pageSize)
                .ProjectTo<ChatDto>(mapper.ConfigurationProvider).ToListAsync();

            return new PagedEntities<ChatDto>(pageChat)
            {
                Quantity = quantity,
                Pages = (int)Math.Ceiling((double)quantity / pageSize)
            };
        }
    }
}
