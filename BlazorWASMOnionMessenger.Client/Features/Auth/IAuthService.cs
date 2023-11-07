using BlazorWASMOnionMessenger.Domain.DTOs.Auth;

namespace BlazorWASMOnionMessenger.Client.Features.Auth
{
    public interface IAuthService
    {
        Task<AuthDto> Register(RegisterDto userRegisterDto);
        Task<AuthDto> Login(LoginDto userLoginDto);
        Task Logout();
    }
}
