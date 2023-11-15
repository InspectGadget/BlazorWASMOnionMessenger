using BlazorWASMOnionMessenger.Application.Interfaces.Repositories;
using BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Persistence.Contexts;
using BlazorWASMOnionMessenger.Persistence.Repositories;
using System.Collections;

namespace BlazorWASMOnionMessenger.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private Hashtable repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (repositories == null)
                repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), dbContext);

                repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)repositories[type];
        }

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
