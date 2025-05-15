using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // Query methods
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        // Add methods
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        // Remove methods
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        // Update method
        void Update(TEntity entity);
    }
} 