namespace BlazorWASMOnionMessenger.Domain.DTOs.Message
{
    public class NewMessageDto
    {
        public int ChatId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string MessageText { get; set; } = string.Empty;
        public string AttachmentUrl { get; set; } = string.Empty;
    }
}
