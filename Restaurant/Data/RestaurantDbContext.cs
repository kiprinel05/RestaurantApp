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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }

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

            // Configure one-to-many relationship between User and Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Order and OrderItems
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Product and OrderItems
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Menu and OrderItems
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Menu)
                .WithMany()
                .HasForeignKey(oi => oi.MenuId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add unique constraint for order code
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderCode)
                .IsUnique();

            // Seed AppSettings
            modelBuilder.Entity<AppSettings>().HasData(
                new AppSettings
                {
                    Id = 1,
                    MenuDiscountPercentage = 10.00M, // 10% reducere pentru meniuri
                    OrderDiscountThreshold = 200.00M, // Reducere pentru comenzi peste 200 lei
                    OrderCountForDiscount = 3, // Reducere după 3 comenzi
                    OrderTimeWindowHours = 24, // În ultimele 24 de ore
                    FreeDeliveryThreshold = 150.00M, // Livrare gratuită peste 150 lei
                    DeliveryCost = 15.00M, // Cost livrare 15 lei
                    LowStockThreshold = 10, // Alertă stoc < 10 porții
                    OrderDiscountPercentage = 15.00M, // 15% reducere pentru comenzi
                    EstimatedDeliveryTimeMinutes = 45 // 45 minute timp estimat livrare
                }
            );

            // Seed test users
            var testUsers = new[]
            {
                new User
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Customer",
                    Email = "customer@test.com",
                    PasswordHash = HashPassword("test123"),
                    PhoneNumber = "0722222222",
                    DeliveryAddress = "Test Address",
                    Role = UserRole.Customer
                },
                new User
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "Employee",
                    Email = "employee@test.com",
                    PasswordHash = HashPassword("test123"),
                    PhoneNumber = "0733333333",
                    DeliveryAddress = "Restaurant Address",
                    Role = UserRole.Employee
                }
            };

            modelBuilder.Entity<User>().HasData(testUsers);

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
                    PrepTime = 20,
                    PortionQuantity = 450, // 450g per portie
                    TotalQuantity = 4500 // 10 portii disponibile
                },
                new Product 
                { 
                    Id = 2, 
                    Name = "Carbonara", 
                    Description = "Spaghetti with eggs, pecorino cheese, guanciale, and black pepper",
                    Price = 35.00M,
                    CategoryId = 2,
                    IsAvailable = true,
                    PrepTime = 15,
                    PortionQuantity = 350, // 350g per portie
                    TotalQuantity = 3500 // 10 portii disponibile
                },
                new Product 
                { 
                    Id = 3, 
                    Name = "Tiramisu", 
                    Description = "Classic Italian dessert with coffee-soaked ladyfingers and mascarpone cream",
                    Price = 25.00M,
                    CategoryId = 3,
                    IsAvailable = true,
                    PrepTime = 10,
                    PortionQuantity = 200, // 200g per portie
                    TotalQuantity = 2000 // 10 portii disponibile
                },
                new Product 
                { 
                    Id = 4, 
                    Name = "Quattro Formaggi", 
                    Description = "Pizza with four different types of cheese",
                    Price = 50.00M,
                    CategoryId = 1,
                    IsAvailable = true,
                    PrepTime = 20,
                    PortionQuantity = 450, // 450g per portie
                    TotalQuantity = 4500 // 10 portii disponibile
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