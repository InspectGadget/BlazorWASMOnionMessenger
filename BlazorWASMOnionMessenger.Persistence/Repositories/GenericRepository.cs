using BlazorWASMOnionMessenger.Application.Common.Exceptions;
using BlazorWASMOnionMessenger.Application.Interfaces.Repositories;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(T entity)
        {
            try
            {
                dbContext.Set<T>().Add(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error occurred while adding entity to the database.", ex);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                dbContext.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error occurred while deleting entity from the database.", ex);
            }
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            try
            {
                IQueryable<T> query = dbContext.Set<T>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error occurred while retrieving entities from the database.", ex);
            }
        }

        public IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            try
            {
                IQueryable<T> query = dbContext.Set<T>().AsQueryable();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                return query;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error occurred while creating a queryable for entities.", ex);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Error occurred while retrieving entity with ID {id} from the database.", ex);
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                dbContext.Set<T>().RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error occurred while deleting a range of entities from the database.", ex);
            }
        }
    }
}
