using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class UnreadMessage : BaseEntity
    {
        public int User_id { get; set; }
        public User User { get; set; } = null!;
        public int Message_id { get; set; }
        public Message Message { get; set; } = null!;
    }
}
