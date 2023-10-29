using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Role> Roles { get; } = new List<Role>();

    }
}
