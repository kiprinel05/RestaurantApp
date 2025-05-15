using Restaurant.Data.Repositories;
using Restaurant.Models;
using System.Threading.Tasks;

namespace Restaurant.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantDbContext _context;
        private IDishRepository _dishes;
        private IRepository<Category> _categories;
        private IRepository<Menu> _menus;
        private IRepository<MenuDish> _menuDishes;
        private IRepository<Allergen> _allergens;
        private IRepository<DishImage> _dishImages;
        private IRepository<User> _users;
        private IOrderRepository _orders;
        private IRepository<OrderItem> _orderItems;

        public UnitOfWork(RestaurantDbContext context)
        {
            _context = context;
        }

        public IDishRepository Dishes => _dishes ??= new DishRepository(_context);
        public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);
        public IRepository<Menu> Menus => _menus ??= new Repository<Menu>(_context);
        public IRepository<MenuDish> MenuDishes => _menuDishes ??= new Repository<MenuDish>(_context);
        public IRepository<Allergen> Allergens => _allergens ??= new Repository<Allergen>(_context);
        public IRepository<DishImage> DishImages => _dishImages ??= new Repository<DishImage>(_context);
        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
        public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 