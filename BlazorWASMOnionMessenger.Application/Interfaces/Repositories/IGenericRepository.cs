using BlazorWASMOnionMessenger.Domain.Common;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        /*
        Task<PagedEntities<T>> GetPageAsync(Expression<Func<T, T>> select, int page, int pageSize, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<PagedEntities<T>> FindPageAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> select, 
            int page, int pageSize, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        */
    }
}
