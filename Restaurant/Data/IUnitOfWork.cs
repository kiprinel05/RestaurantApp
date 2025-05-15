using Restaurant.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace Restaurant.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDishRepository Dishes { get; }
        IRepository<Category> Categories { get; }
        IRepository<Menu> Menus { get; }
        IRepository<MenuDish> MenuDishes { get; }
        IRepository<Allergen> Allergens { get; }
        IRepository<DishImage> DishImages { get; }
        IRepository<User> Users { get; }
        IOrderRepository Orders { get; }
        IRepository<OrderItem> OrderItems { get; }

        Task<int> CompleteAsync();
    }
} 