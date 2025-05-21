using Restaurant.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<List<Allergen>> GetAllergensAsync();
        Task UpdateProductAllergensAsync(int productId, List<int> allergenIds);
        Task<string> SaveProductImageAsync(int productId, string base64Image, string fileName);
        Task DeleteProductImageAsync(int productId, string imagePath);
        Task<string> AddProductImageAsync(int productId, string sourceFilePath);
        Task RemoveProductImageAsync(int productId, string imagePath);
    }
} 