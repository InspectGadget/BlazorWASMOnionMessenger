namespace BlazorWASMOnionMessenger.Domain.DTOs.User
{
    public class UserChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
