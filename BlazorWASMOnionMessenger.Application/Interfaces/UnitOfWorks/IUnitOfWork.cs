using BlazorWASMOnionMessenger.Application.Interfaces.Repositories;
using BlazorWASMOnionMessenger.Domain.Common;

namespace BlazorWASMOnionMessenger.Application.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveAsync();
    }
}
