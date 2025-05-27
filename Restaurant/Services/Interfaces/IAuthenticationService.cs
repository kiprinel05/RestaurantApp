using Restaurant.Models;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface IAuthenticationService
    {
        Task<User?> LoginAsync(string email, string password);
        Task<(bool success, string message)> RegisterAsync(string firstName, string lastName, string email, string password, string phoneNumber, string deliveryAddress);
        Task<bool> IsEmailAvailableAsync(string email);
        User? GetCurrentUser();
        void Logout();
        bool IsUserInRole(string role);
        bool IsAuthenticated { get; }
    }
} 