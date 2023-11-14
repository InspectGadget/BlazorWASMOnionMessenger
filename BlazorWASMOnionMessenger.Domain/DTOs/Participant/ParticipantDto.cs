using BlazorWASMOnionMessenger.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWASMOnionMessenger.Domain.DTOs.Participant
{
    public class ParticipantDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string ChatName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
