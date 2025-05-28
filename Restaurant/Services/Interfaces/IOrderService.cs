using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Models;

namespace Restaurant.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(Order order);
        Task<List<Order>> GetOrdersForUserAsync(int userId);
        Task<bool> CancelOrderAsync(int orderId, int userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<List<Order>> GetAllOrdersForAdminAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetActiveOrdersAsync(int userId);
    }
} 