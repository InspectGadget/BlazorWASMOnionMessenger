using AutoMapper;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using BlazorWASMOnionMessenger.Domain.DTOs.User;
using BlazorWASMOnionMessenger.Domain.Entities;

namespace BlazorWASMOnionMessenger.Application.Common.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
