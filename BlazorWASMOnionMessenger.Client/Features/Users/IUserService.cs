using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.Auth;
using BlazorWASMOnionMessenger.Domain.DTOs.User;

namespace BlazorWASMOnionMessenger.Client.Features.Users
{
    public interface IUserService
    {
        Task<AuthDto> ChangePassword(UserChangePasswordDto userChangePasswordDto);
        Task<UserDto> GetById(string userId);
        Task<ResponseDto> Update(UserDto userDto);
        Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search);
    }
}
