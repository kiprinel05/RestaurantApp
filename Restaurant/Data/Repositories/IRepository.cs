using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Query methods
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Add methods
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // Remove methods
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // Update method
        void Update(T entity);
    }
} 