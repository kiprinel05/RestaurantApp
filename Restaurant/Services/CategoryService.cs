using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly RestaurantDbContext _context;

        public CategoryService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .Include(c => c.Menus)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .Include(c => c.Menus)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"Category with ID {category.Id} not found");

            _context.Entry(existingCategory).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .Include(c => c.Menus)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            if (category.Products.Any() || category.Menus.Any())
                throw new InvalidOperationException("Cannot delete category that has products or menus");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = _context.Categories.AsQueryable();
            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);

            return !await query.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
} 