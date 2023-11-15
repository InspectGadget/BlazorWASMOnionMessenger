using AutoMapper;
using BlazorWASMOnionMessenger.Domain.DTOs.Message;
using BlazorWASMOnionMessenger.Domain.Entities;

namespace BlazorWASMOnionMessenger.Application.Common.MappingProfiles
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile() 
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(dest => dest.ChatName, opt => opt.MapFrom(src => src.Chat.Name));

            CreateMap<CreateMessageDto, Message>()
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
