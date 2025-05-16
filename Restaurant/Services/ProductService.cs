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
        private readonly RestaurantDbContext _context;
        private readonly string _imageDirectory;

        public ProductService(RestaurantDbContext context)
        {
            _context = context;
            _imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Products");
            Directory.CreateDirectory(_imageDirectory);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (await _context.Products.AnyAsync(p => p.Name == product.Name))
            {
                throw new InvalidOperationException("A product with this name already exists.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            if (await _context.Products.AnyAsync(p => p.Name == product.Name && p.Id != product.Id))
            {
                throw new InvalidOperationException("A product with this name already exists.");
            }

            // Update basic properties
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.PortionQuantity = product.PortionQuantity;
            existingProduct.TotalQuantity = product.TotalQuantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.IsAvailable = product.IsAvailable;
            existingProduct.PrepTime = product.PrepTime;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            // Delete associated images from disk
            foreach (var image in product.Images)
            {
                var imagePath = Path.Combine(_imageDirectory, Path.GetFileName(image.ImagePath));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Allergens)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            return !await _context.Products
                .AnyAsync(p => p.Name == name && (!excludeId.HasValue || p.Id != excludeId.Value));
        }

        public async Task<List<Allergen>> GetAllergensAsync()
        {
            return await _context.Allergens.ToListAsync();
        }

        public async Task UpdateProductAllergensAsync(int productId, List<int> allergenIds)
        {
            var product = await _context.Products
                .Include(p => p.Allergens)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            var allergens = await _context.Allergens
                .Where(a => allergenIds.Contains(a.Id))
                .ToListAsync();

            product.Allergens.Clear();
            foreach (var allergen in allergens)
            {
                product.Allergens.Add(allergen);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> SaveProductImageAsync(int productId, string base64Image, string fileName)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            // Generate unique filename
            var extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine("Images", "Products", uniqueFileName);
            var fullPath = Path.Combine(_imageDirectory, uniqueFileName);

            // Save image to disk
            var imageBytes = Convert.FromBase64String(base64Image);
            await File.WriteAllBytesAsync(fullPath, imageBytes);

            // Save image path to database
            var productImage = new ProductImage
            {
                ImagePath = relativePath,
                ProductId = productId
            };

            product.Images.Add(productImage);
            await _context.SaveChangesAsync();

            return relativePath;
        }

        public async Task DeleteProductImageAsync(int productId, string imagePath)
        {
            var product = await _context.Products
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

            // Delete file from disk
            var fullPath = Path.Combine(_imageDirectory, Path.GetFileName(imagePath));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            // Remove from database
            product.Images.Remove(image);
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
} 