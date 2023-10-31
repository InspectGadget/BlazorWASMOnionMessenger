using BlazorWASMOnionMessenger.Domain.DTOs.User;

namespace BlazorWASMOnionMessenger.Client.Features.Users
{
    public interface IUserService
    {
        Task<UserResponseDto> Register(UserRegisterDto userRegisterDto);
        Task<UserResponseDto> Login(UserLoginDto userLoginDto);
        Task<UserResponseDto> ChangePassword(UserChangePasswordDto userChangePasswordDto);
        Task Logout();
    }
}
