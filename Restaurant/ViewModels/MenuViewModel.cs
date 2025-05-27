using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Models;
using Restaurant.Services;
using Restaurant.Views.Menu;
using Restaurant.Views.Cart;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace Restaurant.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        private readonly IMenuService _menuService;
        private readonly IServiceProvider? _serviceProvider;
        private readonly ICartService _cartService;
        private List<Category> _allCategories = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string searchQuery = string.Empty;

        [ObservableProperty]
        private string contentTitle = "Menu";

        [ObservableProperty]
        private object currentContent;

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();

        public MenuViewModel(IMenuService menuService, IServiceProvider? serviceProvider = null)
        {
            _menuService = menuService;
            _serviceProvider = serviceProvider;
            _cartService = serviceProvider?.GetRequiredService<ICartService>();
            LoadMenuAsync();
        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterMenu();
        }

        private async void LoadMenuAsync()
        {
            IsLoading = true;
            try
            {
                _allCategories = await _menuService.GetAllCategoriesWithDetailsAsync();
                FilterMenu();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterMenu()
        {
            if (_allCategories == null) return;

            var filteredCategories = _allCategories.ToList();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var searchTerm = SearchQuery.ToLower();
                filteredCategories = filteredCategories
                    .Select(c => new Category
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Products = c.Products.Where(p =>
                            p.Name.ToLower().Contains(searchTerm) ||
                            p.Description.ToLower().Contains(searchTerm) ||
                            (p.Allergens != null && p.Allergens.Any(a => a.Name.ToLower().Contains(searchTerm)))
                        ).ToList(),
                        Menus = c.Menus.Where(m =>
                            m.Name.ToLower().Contains(searchTerm) ||
                            m.Description.ToLower().Contains(searchTerm) ||
                            (m.MenuProducts != null && m.MenuProducts.Any(mp =>
                                mp.Product != null && mp.Product.Allergens != null &&
                                mp.Product.Allergens.Any(a => a.Name.ToLower().Contains(searchTerm))
                            ))
                        ).ToList()
                    })
                    .Where(c => c.Products.Any() || c.Menus.Any())
                    .ToList();
            }

            Categories.Clear();
            foreach (var category in filteredCategories)
            {
                Categories.Add(category);
            }
        }

        public void NavigateToMenu()
        {
            ContentTitle = "Menu";
            if (_serviceProvider != null)
            {
                var menuContentView = _serviceProvider.GetRequiredService<MenuContentView>();
                menuContentView.DataContext = this;
                CurrentContent = menuContentView;
            }
        }

        public void NavigateToOrders()
        {
            ContentTitle = "My Orders";
            if (_serviceProvider != null)
            {
                CurrentContent = _serviceProvider.GetRequiredService<OrdersView>();
            }
        }

        public void NavigateToCart()
        {
            ContentTitle = "Shopping Cart";
            if (_serviceProvider != null)
            {
                CurrentContent = _serviceProvider.GetRequiredService<CartView>();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (_serviceProvider == null) return false;
                var authService = _serviceProvider.GetService(typeof(IAuthenticationService)) as IAuthenticationService;
                return authService?.IsAuthenticated ?? false;
            }
        }

        public RelayCommand<object> AddToCartCommand => new RelayCommand<object>(item =>
        {
            if (_cartService == null || item == null) return;
            if (item is Product product)
                _cartService.AddToCart(product);
            else if (item is Menu menu)
                _cartService.AddToCart(menu);
        });
    }
} 