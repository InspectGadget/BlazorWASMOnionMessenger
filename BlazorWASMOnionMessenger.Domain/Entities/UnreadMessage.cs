using BlazorWASMOnionMessenger.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class UnreadMessage : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [ForeignKey("Message")]
        public int MessageId { get; set; }
        public Message Message { get; set; } = null!;
    }
}
