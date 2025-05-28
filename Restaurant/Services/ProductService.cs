using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class ProductService : IProductService
    {
        private readonly IDbContextFactory<RestaurantDbContext> _contextFactory;
        private readonly string _imageDirectory;

        public ProductService(IDbContextFactory<RestaurantDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Products");
            Directory.CreateDirectory(_imageDirectory);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            using var context = _contextFactory.CreateDbContext();
            if (await context.Products.AnyAsync(p => p.Name == product.Name))
            {
                throw new InvalidOperationException("A product with this name already exists");
            }

            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            using var context = _contextFactory.CreateDbContext();
            var existingProduct = await context.Products
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            if (await context.Products.AnyAsync(p => p.Name == product.Name && p.Id != product.Id))
            {
                throw new InvalidOperationException("A product with this name already exists");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.PortionQuantity = product.PortionQuantity;
            existingProduct.TotalQuantity = product.TotalQuantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.IsAvailable = product.IsAvailable;
            existingProduct.PrepTime = product.PrepTime;

            await context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task DeleteProductAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            foreach (var image in product.Images)
            {
                var imagePath = Path.Combine(_imageDirectory, Path.GetFileName(image.ImagePath));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Products
                .Include(p => p.Category)
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            using var context = _contextFactory.CreateDbContext();
            return !await context.Products
                .AnyAsync(p => p.Name == name && (!excludeId.HasValue || p.Id != excludeId.Value));
        }

        public async Task<List<Allergen>> GetAllergensAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Allergens.ToListAsync();
        }

        public async Task UpdateProductAllergensAsync(int productId, List<int> allergenIds)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products
                .Include(p => p.Allergens)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            var allergens = await context.Allergens
                .Where(a => allergenIds.Contains(a.Id))
                .ToListAsync();

            product.Allergens.Clear();
            foreach (var allergen in allergens)
            {
                product.Allergens.Add(allergen);
            }

            await context.SaveChangesAsync();
        }

        public async Task<string> SaveProductImageAsync(int productId, string base64Image, string fileName)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine("Images", "Products", uniqueFileName);
            var fullPath = Path.Combine(_imageDirectory, uniqueFileName);

            var imageBytes = Convert.FromBase64String(base64Image);
            await File.WriteAllBytesAsync(fullPath, imageBytes);

            var productImage = new ProductImage
            {
                ImagePath = relativePath,
                ProductId = productId
            };

            product.Images.Add(productImage);
            await context.SaveChangesAsync();

            return relativePath;
        }

        public async Task DeleteProductImageAsync(int productId, string imagePath)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            var image = product.Images.FirstOrDefault(i => i.ImagePath == imagePath);
            if (image == null)
            {
                throw new InvalidOperationException("Image not found");
            }

            var fullPath = Path.Combine(_imageDirectory, Path.GetFileName(imagePath));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            product.Images.Remove(image);
            context.ProductImages.Remove(image);
            await context.SaveChangesAsync();
        }

        public async Task<string> AddProductImageAsync(int productId, string sourceFilePath)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            var extension = Path.GetExtension(sourceFilePath);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine("Images", "Products", uniqueFileName);
            var fullPath = Path.Combine(_imageDirectory, uniqueFileName);

            File.Copy(sourceFilePath, fullPath, true);

            var productImage = new ProductImage
            {
                ImagePath = relativePath,
                ProductId = productId
            };

            product.Images.Add(productImage);
            await context.SaveChangesAsync();

            return relativePath;
        }

        public async Task RemoveProductImageAsync(int productId, string imagePath)
        {
            using var context = _contextFactory.CreateDbContext();
            var product = await context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            var image = product.Images.FirstOrDefault(i => i.ImagePath == imagePath);
            if (image == null)
            {
                throw new InvalidOperationException("Image not found.");
            }

            var fullPath = Path.Combine(_imageDirectory, Path.GetFileName(imagePath));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            product.Images.Remove(image);
            context.ProductImages.Remove(image);
            await context.SaveChangesAsync();
        }
    }
} 