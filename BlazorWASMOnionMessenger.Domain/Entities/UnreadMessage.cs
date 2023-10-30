using BlazorWASMOnionMessenger.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class UnreadMessage : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey("Message")]
        public int MessageId { get; set; }
        public Message Message { get; set; } = null!;
    }
}
