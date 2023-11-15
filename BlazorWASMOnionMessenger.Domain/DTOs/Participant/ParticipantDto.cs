namespace BlazorWASMOnionMessenger.Domain.DTOs.Participant
{
    public class ParticipantDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
