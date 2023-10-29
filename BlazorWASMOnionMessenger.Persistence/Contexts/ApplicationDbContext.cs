using BlazorWASMOnionMessenger.Application.Interfaces.Contexts;
using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWASMOnionMessenger.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Chat> ChatTypes { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UnreadMessage> UnreadMessages { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
