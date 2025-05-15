using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersWithDetailsAsync();
        Task<Order> GetOrderWithDetailsAsync(int id);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<Order>> GetActiveOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetUserOrderCountInPeriodAsync(int userId, DateTime startDate, DateTime endDate);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
} 