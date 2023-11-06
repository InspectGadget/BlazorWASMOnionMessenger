using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.DTOs.User
{
    public class UserAuthDto : ResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
