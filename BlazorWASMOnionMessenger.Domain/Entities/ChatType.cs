using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class ChatType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Chat> Chats { get; } = new List<Chat>();
    }
}
