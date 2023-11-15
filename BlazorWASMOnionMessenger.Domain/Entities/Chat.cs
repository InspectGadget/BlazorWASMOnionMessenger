using BlazorWASMOnionMessenger.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        [ForeignKey("ChatType")]
        public int ChatTypeId { get; set; }
        public ChatType ChatType { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public ICollection<Participant> Participants { get; } = new List<Participant>();
        public ICollection<Message> Messages { get; } = new List<Message>();

    }
}
