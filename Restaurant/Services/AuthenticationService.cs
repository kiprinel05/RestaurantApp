using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly RestaurantDbContext _context;

        public AuthenticationService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword) return null;

            return user;
        }

        public async Task<(bool success, string message)> RegisterAsync(string firstName, string lastName, 
            string email, string password, string phoneNumber, string deliveryAddress)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return (false, "Email already exists");
            }

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = HashPassword(password),
                PhoneNumber = phoneNumber,
                DeliveryAddress = deliveryAddress,
                Role = UserRole.Customer
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "Registration successful");
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !await _context.Users.AnyAsync(u => u.Email == email);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
} 