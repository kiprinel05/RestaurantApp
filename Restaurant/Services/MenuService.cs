using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class MenuService : IMenuService
    {
        private readonly IDbContextFactory<RestaurantDbContext> _contextFactory;

        public MenuService(IDbContextFactory<RestaurantDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Category>> GetAllCategoriesWithDetailsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var categories = await context.Categories
                .Include(c => c.Products)
                    .ThenInclude(p => p.Allergens)
                .Include(c => c.Products)
                    .ThenInclude(p => p.Images)
                .Include(c => c.Menus)
                    .ThenInclude(m => m.MenuProducts)
                        .ThenInclude(mp => mp.Product)
                .ToListAsync();

            // Ensure all menus have valid image paths
            foreach (var category in categories)
            {
                foreach (var menu in category.Menus)
                {
                    if (string.IsNullOrEmpty(menu.ImagePath))
                    {
                        menu.ImagePath = "/Images/default-menu.jpg";
                    }
                }
            }

            return categories;
        }

        public async Task<List<Category>> SearchMenuAsync(string searchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            
            var matchingProducts = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                           p.Description.ToLower().Contains(searchTerm) ||
                           p.Allergens.Any(a => a.Name.ToLower().Contains(searchTerm)))
                .ToListAsync();

            var matchingMenus = await context.Menus
                .Include(m => m.Category)
                .Include(m => m.MenuProducts)
                    .ThenInclude(mp => mp.Product)
                .Where(m => m.Name.ToLower().Contains(searchTerm) ||
                           m.Description.ToLower().Contains(searchTerm))
                .ToListAsync();

            // Ensure all menus have valid image paths
            foreach (var menu in matchingMenus)
            {
                if (string.IsNullOrEmpty(menu.ImagePath))
                {
                    menu.ImagePath = "/Images/default-menu.jpg";
                }
            }

            // Combine unique categories that contain either matching products or menus
            var categoriesWithProducts = matchingProducts.GroupBy(p => p.Category);
            var categoriesWithMenus = matchingMenus.GroupBy(m => m.Category);
            
            return categoriesWithProducts.Select(g => g.Key)
                .Union(categoriesWithMenus.Select(g => g.Key))
                .ToList();
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var menus = await context.Menus
                .Include(m => m.Category)
                .Include(m => m.MenuProducts)
                    .ThenInclude(mp => mp.Product)
                .ToListAsync();
            return menus;
        }

        public async Task DeleteMenuAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var menu = await context.Menus.FindAsync(id);
            if (menu != null)
            {
                context.Menus.Remove(menu);
                await context.SaveChangesAsync();
            }
        }
    }
} 