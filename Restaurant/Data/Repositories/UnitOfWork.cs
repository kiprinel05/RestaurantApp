using Restaurant.Models;
using System.Threading.Tasks;

namespace Restaurant.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantDbContext _context;
        private IRepository<Allergen>? _allergens;
        private IRepository<Category>? _categories;
        private IRepository<Dish>? _dishes;
        private IRepository<DishImage>? _dishImages;
        private IRepository<Menu>? _menus;
        private IRepository<MenuDish>? _menuDishes;
        private IRepository<Order>? _orders;
        private IRepository<OrderItem>? _orderItems;
        private IRepository<User>? _users;

        public UnitOfWork(RestaurantDbContext context)
        {
            _context = context;
        }

        public IRepository<Allergen> Allergens => _allergens ??= new Repository<Allergen>(_context);
        public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
        public IRepository<Dish> Dishes => _dishes ??= new Repository<Dish>(_context);
        public IRepository<DishImage> DishImages => _dishImages ??= new Repository<DishImage>(_context);
        public IRepository<Menu> Menus => _menus ??= new Repository<Menu>(_context);
        public IRepository<MenuDish> MenuDishes => _menuDishes ??= new Repository<MenuDish>(_context);
        public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);
        public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);
        public IRepository<User> Users => _users ??= new Repository<User>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 