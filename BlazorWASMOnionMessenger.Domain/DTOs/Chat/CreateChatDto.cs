namespace BlazorWASMOnionMessenger.Domain.DTOs.Chat
{
    public class CreateChatDto
    {
        public string Name { get; set; } = string.Empty;
        public int ChatTypeId { get; set; }
        public string CreatorId { get; set; } = string.Empty;
        public string? ParticipantId { get; set; }
    }
}
