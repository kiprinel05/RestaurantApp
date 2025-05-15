using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        public DishRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Dish>> GetDishesWithDetailsAsync()
        {
            return await Context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .ToListAsync();
        }

        public async Task<Dish> GetDishWithDetailsAsync(int id)
        {
            return await Context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .SingleOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Dish>> SearchDishesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetDishesWithDetailsAsync();

            return await Context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .Where(d => d.Name.Contains(searchTerm) || 
                           d.Description.Contains(searchTerm) ||
                           d.Category.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Dish>> GetDishesByAllergenAsync(string allergenName, bool excludeAllergen = false)
        {
            var query = Context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Allergens)
                .Include(d => d.Images);

            if (excludeAllergen)
            {
                return await query
                    .Where(d => !d.Allergens.Any(a => a.Name.Contains(allergenName)))
                    .ToListAsync();
            }
            else
            {
                return await query
                    .Where(d => d.Allergens.Any(a => a.Name.Contains(allergenName)))
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Dish>> GetLowStockDishesAsync(double threshold)
        {
            return await Context.Dishes
                .Where(d => d.TotalQuantityAvailable <= threshold)
                .OrderBy(d => d.TotalQuantityAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Dish>> GetDishesByCategoryAsync(int categoryId)
        {
            return await Context.Dishes
                .Include(d => d.Allergens)
                .Include(d => d.Images)
                .Where(d => d.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task UpdateStockAsync(int dishId, double quantityChange)
        {
            var dish = await Context.Dishes.FindAsync(dishId);
            if (dish != null)
            {
                dish.TotalQuantityAvailable += quantityChange;
                Context.Dishes.Update(dish);
            }
        }
    }
} 