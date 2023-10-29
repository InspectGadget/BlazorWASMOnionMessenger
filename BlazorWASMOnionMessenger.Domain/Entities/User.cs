using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string First_name { get; set; } = string.Empty;
        public string Last_name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set;}
        public ICollection<Message> Messages { get; } = new List<Message>();
        public ICollection<Participant> Participants { get; } = new List<Participant>();
        public ICollection<UnreadMessage> UnreadMessages { get; } = new List<UnreadMessage>();
    }
}
