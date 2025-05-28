using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Restaurant.Data;
using Restaurant.Services;
using Restaurant.Services.Interfaces;
using Restaurant.ViewModels;
using Restaurant.Views;
using Restaurant.Views.Product;
using System;
using System.Windows;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Initialize the database
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }

        public T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>()!;
        }

        public T GetRequiredService<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Configurare IConfiguration
            services.AddSingleton<IConfiguration>(sp => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build());

            // Database configuration
            services.AddDbContextFactory<RestaurantDbContext>(options =>
                options.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=RestaurantT3DB;Trusted_Connection=True;MultipleActiveResultSets=true"));

            // Register services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IAllergenService, AllergenService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<AppSettingsProvider>();
            
            // Register ViewModels
            services.AddTransient<AuthViewModel>();
            services.AddTransient<MenuViewModel>(sp => new MenuViewModel(
                sp.GetRequiredService<IMenuService>(),
                sp));
            services.AddTransient<CartViewModel>(sp => new CartViewModel(
                sp.GetRequiredService<ICartService>(),
                sp.GetRequiredService<IOrderService>(),
                sp.GetRequiredService<IAuthenticationService>(),
                sp.GetRequiredService<AppSettingsProvider>()));
            services.AddTransient<MenuListViewModel>();
            services.AddTransient<EmployeeDashboardViewModel>();
            services.AddTransient<CategoriesViewModel>();
            services.AddTransient<CategoryListViewModel>();
            services.AddTransient<CategoryEditViewModel>();
            services.AddTransient<ProductEditViewModel>();
            services.AddTransient<ProductListViewModel>();
            services.AddTransient<ProductAddViewModel>();
            services.AddTransient<MenuAddViewModel>();
            services.AddTransient<OrdersViewModel>();
            services.AddTransient<LowStockViewModel>();

            // Register views with their ViewModels
            services.AddTransient(sp => new AuthView(sp.GetRequiredService<AuthViewModel>()));
            services.AddTransient<Restaurant.Views.Menu.MenuView>();
            services.AddTransient<Restaurant.Views.Menu.MenuContentView>();
            services.AddTransient<Restaurant.Views.Menu.OrdersView>(sp =>
                new Restaurant.Views.Menu.OrdersView { DataContext = sp.GetRequiredService<OrdersViewModel>() });
            services.AddTransient<Restaurant.Views.Cart.CartView>(sp => 
                new Restaurant.Views.Cart.CartView(sp.GetRequiredService<CartViewModel>()));
            services.AddTransient(sp => new EmployeeDashboardView(sp.GetRequiredService<EmployeeDashboardViewModel>()));
            services.AddTransient<Restaurant.Views.Employee.CategoriesView>();
            services.AddTransient(sp => new CategoryListView(sp.GetRequiredService<CategoryListViewModel>()));
            services.AddTransient(sp => new CategoryEditView(sp.GetRequiredService<CategoryEditViewModel>()));
            services.AddTransient(sp => new ProductListView(sp.GetRequiredService<ProductListViewModel>()));
            services.AddTransient(sp => {
                var view = new ProductEditView();
                view.DataContext = sp.GetRequiredService<ProductEditViewModel>();
                return view;
            });
            services.AddTransient(sp => {
                var view = new ProductAddView();
                view.DataContext = sp.GetRequiredService<ProductAddViewModel>();
                return view;
            });
            services.AddTransient(sp => {
                var view = new Restaurant.Views.Menu.MenuListView();
                view.DataContext = sp.GetRequiredService<MenuListViewModel>();
                return view;
            });
            services.AddTransient(sp => {
                var view = new Restaurant.Views.Menu.MenuAddView();
                view.DataContext = sp.GetRequiredService<MenuAddViewModel>();
                return view;
            });

            // Register MainWindow as singleton
            services.AddSingleton<MainWindow>();
            services.AddSingleton<Window>(sp => sp.GetRequiredService<MainWindow>());

            // Register navigation service after MainWindow
            services.AddSingleton<INavigationService>(sp => 
                new NavigationService(
                    sp.GetRequiredService<Window>(),
                    sp,
                    sp.GetRequiredService<IAuthenticationService>()));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
                
                mainWindow.Show();
                navigationService.NavigateToAuth(); // Start with auth view
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting application: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }
    }
}
