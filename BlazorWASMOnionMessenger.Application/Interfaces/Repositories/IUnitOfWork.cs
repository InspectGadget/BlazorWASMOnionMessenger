using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> Save();
    }
}
