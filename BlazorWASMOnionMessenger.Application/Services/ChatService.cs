using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorWASMOnionMessenger.Application.Interfaces.Chats;
using BlazorWASMOnionMessenger.Application.Interfaces.Common;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using BlazorWASMOnionMessenger.Domain.Entities;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace BlazorWASMOnionMessenger.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISearchPredicateBuilder searchPredicateBuilder;
        private readonly IMapper mapper;

        public ChatService(IUnitOfWork unitOfWork, ISearchPredicateBuilder searchPredicateBuilder, IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.searchPredicateBuilder = searchPredicateBuilder;
            this.mapper = mapper;
        }
        public async Task<PagedEntities<ChatDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search)
        {
            var chatRepository = unitOfWork.Repository<Chat>();

            var searchPredicate = searchPredicateBuilder.BuildSearchPredicate<Chat, ChatDto>(search);

            var query = chatRepository.GetAllQueryable(searchPredicate);

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
