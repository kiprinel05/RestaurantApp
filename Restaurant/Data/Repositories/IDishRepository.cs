using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public interface IDishRepository : IRepository<Dish>
    {
        Task<IEnumerable<Dish>> GetDishesWithDetailsAsync();
        Task<Dish> GetDishWithDetailsAsync(int id);
        Task<IEnumerable<Dish>> SearchDishesAsync(string searchTerm);
        Task<IEnumerable<Dish>> GetDishesByAllergenAsync(string allergenName, bool excludeAllergen = false);
        Task<IEnumerable<Dish>> GetLowStockDishesAsync(double threshold);
        Task<IEnumerable<Dish>> GetDishesByCategoryAsync(int categoryId);
        Task UpdateStockAsync(int dishId, double quantityChange);
    }
} 