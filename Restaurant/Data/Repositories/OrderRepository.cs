using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(RestaurantDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersWithDetailsAsync()
        {
            return await Context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Menu)
                        .ThenInclude(m => m.MenuDishes)
                            .ThenInclude(md => md.Dish)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderWithDetailsAsync(int id)
        {
            return await Context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Menu)
                        .ThenInclude(m => m.MenuDishes)
                            .ThenInclude(md => md.Dish)
                .SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await Context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Menu)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetActiveOrdersAsync()
        {
            return await Context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Menu)
                .Where(o => o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Cancelled)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await Context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<int> GetUserOrderCountInPeriodAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await Context.Orders
                .CountAsync(o => o.UserId == userId && 
                                o.OrderDate >= startDate && 
                                o.OrderDate <= endDate);
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await Context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                Context.Orders.Update(order);
            }
        }
    }
} 