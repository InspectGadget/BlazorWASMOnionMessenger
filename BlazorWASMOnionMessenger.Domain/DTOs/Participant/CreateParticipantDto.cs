namespace BlazorWASMOnionMessenger.Domain.DTOs.Participant
{
    public class CreateParticipantDto
    {
        private const int RegularUserId = 2;
        public int ChatId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int? RoleId { get; set; } = RegularUserId;
    }
}
