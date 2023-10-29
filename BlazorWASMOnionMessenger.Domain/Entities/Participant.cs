using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class Participant : BaseEntity
    {
        public int Chat_id { get; set; }
        public Chat Chat { get; set; } = null!;
        public int User_id { get; set; }
        public User User { get; set; } = null!;
        public int Role_id { get; set; }
        public Role Role { get; set; } = null!;
        public DateTime Joined_at { get; set; }
    }
}
