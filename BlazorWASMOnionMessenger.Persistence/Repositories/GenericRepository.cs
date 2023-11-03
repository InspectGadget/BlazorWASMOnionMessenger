using BlazorWASMOnionMessenger.Application.Interfaces.Repositories;
using BlazorWASMOnionMessenger.Domain.Common;
using BlazorWASMOnionMessenger.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace BlazorWASMOnionMessenger.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext
            .Set<T>()
            .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public Task UpdateAsync(T entity)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
    }
}
