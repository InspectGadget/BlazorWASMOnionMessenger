
namespace BlazorWASMOnionMessenger.Domain.DTOs.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string ChatName { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string MessageText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string AttachmentUrl { get; set; } = string.Empty;
    }
}
