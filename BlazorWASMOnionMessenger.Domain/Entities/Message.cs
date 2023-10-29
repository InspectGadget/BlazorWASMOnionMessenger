using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class Message : BaseEntity
    {
        public int Chat_id { get; set; }
        public Chat Chat { get; set; } = null!;
        public int Sender_id { get; set; }
        public User User { get; set; } = null!;
        public string Message_text { get; set; } = string.Empty;
        public DateTime Created_at { get; set; }
        public string Attachment_url { get; set; } = string.Empty;
        public ICollection<UnreadMessage> UnreadMessages { get; } = new List<UnreadMessage>();
    }
}
