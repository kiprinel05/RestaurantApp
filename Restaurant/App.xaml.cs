using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data;
using Restaurant.Services;
using Restaurant.ViewModels;
using Restaurant.Views;
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
            // Database configuration
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=RestaurantT3DB;Trusted_Connection=True;MultipleActiveResultSets=true"));

            // Register services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            
            // Register ViewModels
            services.AddTransient<AuthViewModel>();
            services.AddTransient<MenuListViewModel>();
            services.AddTransient<EmployeeDashboardViewModel>();
            services.AddTransient<CategoryListViewModel>();
            services.AddTransient<CategoryEditViewModel>();
            services.AddTransient<ProductEditViewModel>();
            services.AddTransient<ProductListViewModel>();

            // Register MainWindow as singleton
            services.AddSingleton<MainWindow>();
            services.AddSingleton<Window>(sp => sp.GetRequiredService<MainWindow>());

            // Register navigation service after MainWindow
            services.AddSingleton<INavigationService>(sp => 
                new NavigationService(
                    sp.GetRequiredService<Window>(),
                    sp,
                    sp.GetRequiredService<IAuthenticationService>()));

            // Register views
            services.AddTransient<AuthView>();
            services.AddTransient<MenuView>();
            services.AddTransient<EmployeeDashboardView>();
            services.AddTransient<CategoryListView>();
            services.AddTransient<CategoryEditView>();
            services.AddTransient<ProductListView>();
            services.AddTransient<ProductEditView>();
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
