using BlazorWASMOnionMessenger.Domain.Entities;

namespace BlazorWASMOnionMessenger.Domain.DTOs.Chat
{
    public class ChatDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ChatType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string LastMessagePreview { get; set; } = string.Empty;
        public DateTime LastMessageDate { get; set; }
        public string LastMessageSender { get; set; } = string.Empty;
    }
}
