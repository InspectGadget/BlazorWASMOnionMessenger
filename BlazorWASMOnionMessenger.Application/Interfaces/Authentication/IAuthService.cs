using BlazorWASMOnionMessenger.Domain.DTOs.Auth;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto userLoginDto);
        Task<string> Register(RegisterDto userRegisterDto);
    }
}
