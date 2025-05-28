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

        private string GetProductImagePath(Product product)
        {
            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                return product.ImagePath;
            }

            var categoryFolder = product.Category?.Name?.Replace(" ", "_") ?? "Other";
            var imageName = product.Name.ToLower().Replace(" ", "_");
            return $"/Images/{categoryFolder}/{imageName}.png";
        }

        private string GetMenuImagePath(Menu menu)
        {
            if (!string.IsNullOrEmpty(menu.ImagePath))
            {
                return menu.ImagePath;
            }

            var categoryFolder = menu.Category?.Name?.Replace(" ", "_") ?? "Other";
            return $"/Images/{categoryFolder}/default-menu.jpg";
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

            foreach (var category in categories)
            {
                foreach (var menu in category.Menus)
                {
                    menu.ImagePath = GetMenuImagePath(menu);
                }

                foreach (var product in category.Products)
                {
                    product.ImagePath = GetProductImagePath(product);
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

            foreach (var menu in matchingMenus)
            {
                menu.ImagePath = GetMenuImagePath(menu);
            }

            foreach (var product in matchingProducts)
            {
                product.ImagePath = GetProductImagePath(product);
            }

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

            foreach (var menu in menus)
            {
                menu.ImagePath = GetMenuImagePath(menu);
            }

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