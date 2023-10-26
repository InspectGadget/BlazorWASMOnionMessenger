using Microsoft.EntityFrameworkCore;

namespace BlazorWASMOnionMessenger.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { 
        }
    }
}
