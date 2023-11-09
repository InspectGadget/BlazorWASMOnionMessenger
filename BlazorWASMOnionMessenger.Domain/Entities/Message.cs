using BlazorWASMOnionMessenger.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class Message : BaseEntity
    {
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public Chat Chat { get; set; } = null!;

        [ForeignKey("ApplicationUser")]
        public string SenderId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public string MessageText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string AttachmentUrl { get; set; } = string.Empty;
        public ICollection<UnreadMessage> UnreadMessages { get; } = new List<UnreadMessage>();
    }
}
