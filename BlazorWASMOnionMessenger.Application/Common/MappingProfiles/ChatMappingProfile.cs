using AutoMapper;
using BlazorWASMOnionMessenger.Domain.DTOs.Chat;
using BlazorWASMOnionMessenger.Domain.Entities;

namespace BlazorWASMOnionMessenger.Application.Common.MappingProfiles
{
    public class ChatMappingProfile : Profile
    {
        public ChatMappingProfile() 
        {
            CreateMap<Chat, ChatDto>()
                .ForMember(dest => dest.ChatType, opt => opt.MapFrom(src => src.ChatType.Name))
                .ForMember(dest => dest.LastMessagePreview, opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(msg => msg.CreatedAt)
                    .Select(msg => msg.MessageText.Substring(0, Math.Min(msg.MessageText.Length, 20)))
                    .FirstOrDefault()))
                .ForMember(dest => dest.LastMessageDate, opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(msg => msg.CreatedAt)
                    .Select(msg => msg.CreatedAt)
                    .FirstOrDefault()))
                .ForMember(dest => dest.LastMessageSender, opt => opt.MapFrom(src => src.Messages
                    .OrderByDescending(msg => msg.CreatedAt)
                    .Select(msg => msg.ApplicationUser.UserName)
                    .FirstOrDefault()));
            CreateMap<CreateChatDto, Chat>();
        }
    }
}
