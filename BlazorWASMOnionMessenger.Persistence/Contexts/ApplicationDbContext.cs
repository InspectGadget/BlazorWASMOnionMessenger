using BlazorWASMOnionMessenger.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorWASMOnionMessenger.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Chat> ChatTypes { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UnreadMessage> UnreadMessages { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
