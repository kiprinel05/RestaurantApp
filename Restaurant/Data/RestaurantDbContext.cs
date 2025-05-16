using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System.Security.Cryptography;
using System.Text;

namespace Restaurant.Data
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add unique constraint for email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Add unique constraint for category name
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Add unique constraint for allergen name
            modelBuilder.Entity<Allergen>()
                .HasIndex(a => a.Name)
                .IsUnique();

            // Add unique constraint for product name
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // Add unique constraint for menu name
            modelBuilder.Entity<Menu>()
                .HasIndex(m => m.Name)
                .IsUnique();

            // Configure many-to-many relationship between Product and Allergen
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Allergens)
                .WithMany(a => a.Products);

            // Configure one-to-many relationship between Category and Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Category and Menus
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Menus)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed test user
            var testUser = new User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.com",
                PasswordHash = HashPassword("test123"),
                PhoneNumber = "0722222222",
                DeliveryAddress = "Test Address",
                Role = UserRole.Customer
            };

            modelBuilder.Entity<User>().HasData(testUser);

            // Seed test categories
            var categories = new[]
            {
                new Category { Id = 1, Name = "Pizza", Description = "Italian style pizzas" },
                new Category { Id = 2, Name = "Pasta", Description = "Fresh homemade pasta" },
                new Category { Id = 3, Name = "Desserts", Description = "Sweet treats" }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seed test products
            var products = new[]
            {
                new Product 
                { 
                    Id = 1, 
                    Name = "Margherita Pizza", 
                    Description = "Classic pizza with tomato sauce, mozzarella, and basil",
                    Price = 45.00M,
                    CategoryId = 1,
                    IsAvailable = true,
                    PrepTime = 20
                },
                new Product 
                { 
                    Id = 2, 
                    Name = "Carbonara", 
                    Description = "Spaghetti with eggs, pecorino cheese, guanciale, and black pepper",
                    Price = 35.00M,
                    CategoryId = 2,
                    IsAvailable = true,
                    PrepTime = 15
                },
                new Product 
                { 
                    Id = 3, 
                    Name = "Tiramisu", 
                    Description = "Classic Italian dessert with coffee-soaked ladyfingers and mascarpone cream",
                    Price = 25.00M,
                    CategoryId = 3,
                    IsAvailable = true,
                    PrepTime = 10
                },
                new Product 
                { 
                    Id = 4, 
                    Name = "Quattro Formaggi", 
                    Description = "Pizza with four different types of cheese",
                    Price = 50.00M,
                    CategoryId = 1,
                    IsAvailable = true,
                    PrepTime = 20
                }
            };

            modelBuilder.Entity<Product>().HasData(products);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
} 