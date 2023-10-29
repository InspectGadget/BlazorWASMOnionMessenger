using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Contexts
{
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Chat> Chats { get; set; }
        DbSet<Chat> ChatTypes { get; set; }
        DbSet<Participant> Participants { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<UnreadMessage> UnreadMessages { get; set; }
        DbSet<Role> Roles { get; set; }
    }
}
