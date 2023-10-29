using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> Save();
    }
}
