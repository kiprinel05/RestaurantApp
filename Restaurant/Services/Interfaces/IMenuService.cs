using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface IMenuService
    {
        Task<List<Category>> GetAllCategoriesWithDetailsAsync();
        Task<List<Category>> SearchMenuAsync(string searchTerm);
        Task<List<Menu>> GetAllMenusAsync();
        Task DeleteMenuAsync(int id);
    }
} 