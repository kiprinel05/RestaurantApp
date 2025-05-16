using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data;
using Restaurant.Services;
using Restaurant.ViewModels;
using Restaurant.Views;
using System;
using System.Windows;
using System.Windows.Controls;

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
                context.Database.EnsureCreated();
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

        private void ConfigureServices(IServiceCollection services)
        {
            // Database configuration
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=RestaurantDB;Trusted_Connection=True;MultipleActiveResultSets=true",
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(App).Assembly.FullName)));

            // Register services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<Frame>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Register view models
            services.AddTransient<AuthViewModel>();

            // Register views
            services.AddTransient<AuthView>();
            services.AddTransient<MainView>();

            // Register main window
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();

                // Get the main frame and navigation service
                var mainFrame = _serviceProvider.GetRequiredService<Frame>();
                var navigationService = _serviceProvider.GetRequiredService<INavigationService>();

                // Navigate to the auth view
                var authView = _serviceProvider.GetRequiredService<AuthView>();
                mainFrame.Navigate(authView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting application: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }
    }
}
