using Restaurant.Models;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Allergen> Allergens { get; }
        IRepository<Category> Categories { get; }
        IRepository<Dish> Dishes { get; }
        IRepository<DishImage> DishImages { get; }
        IRepository<Menu> Menus { get; }
        IRepository<MenuDish> MenuDishes { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<User> Users { get; }

        Task<int> CompleteAsync();
    }
} 