using BlazorWASMOnionMessenger.Domain.DTOs.User;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<string> Login(UserLoginDto userLoginDto);
        Task<string> Register(UserRegisterDto userRegisterDto);
        Task ChangePassword(string userId, string currentPassword, string newPassword);
    }
}
