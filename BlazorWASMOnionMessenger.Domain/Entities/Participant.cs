using BlazorWASMOnionMessenger.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class Participant : BaseEntity
    {
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public Chat Chat { get; set; } = null!;

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public DateTime JoinedAt { get; set; }
    }
}
