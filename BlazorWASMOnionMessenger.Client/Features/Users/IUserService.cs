using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Domain.DTOs.User;

namespace BlazorWASMOnionMessenger.Client.Features.Users
{
    public interface IUserService
    {
        Task<UserAuthDto> Register(UserRegisterDto userRegisterDto);
        Task<UserAuthDto> Login(UserLoginDto userLoginDto);
        Task<UserAuthDto> ChangePassword(UserChangePasswordDto userChangePasswordDto);
        Task Logout();
        Task<UserDto> GetById(string userId);
        Task<ResponseDto> Update(UserDto userDto);
        Task<PagedEntities<UserDto>> GetPage(int page, int pageSize, string orderBy, bool orderType, string search);
    }
}
