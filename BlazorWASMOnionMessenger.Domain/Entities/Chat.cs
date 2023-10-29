using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public int Type_id { get; set; }
        public ChatType ChatType { get; set; } = null!;
        public DateTime Created_at { get; set; }
        public ICollection<Participant> Participants { get; } = new List<Participant>();
        public ICollection<Message> Messages { get; } = new List<Message>();

    }
}
