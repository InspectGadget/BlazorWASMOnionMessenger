using Microsoft.AspNetCore.Identity;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Message> Messages { get; } = new List<Message>();
        public ICollection<Participant> Participants { get; } = new List<Participant>();
        public ICollection<UnreadMessage> UnreadMessages { get; } = new List<UnreadMessage>();
    }
}
