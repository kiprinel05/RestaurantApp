using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbContextFactory<RestaurantDbContext> _contextFactory;

        public CategoryService(IDbContextFactory<RestaurantDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Categories
                .Include(c => c.Products)
                .Include(c => c.Menus)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Categories
                .Include(c => c.Products)
                .Include(c => c.Menus)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            using var context = _contextFactory.CreateDbContext();
            var existingCategory = await context.Categories.FindAsync(category.Id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"Category with ID {category.Id} not found");

            context.Entry(existingCategory).CurrentValues.SetValues(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var category = await context.Categories
                .Include(c => c.Products)
                .Include(c => c.Menus)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            if (category.Products.Any() || category.Menus.Any())
                throw new InvalidOperationException("Cannot delete category that has products or menus");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string name, int? excludeId = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = context.Categories.AsQueryable();
            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);

            return !await query.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
} 