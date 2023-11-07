using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.DTOs.Auth
{
    public class AuthDto : ResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
