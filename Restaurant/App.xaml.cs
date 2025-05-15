using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Data;
using Restaurant.Data.Repositories;
using Restaurant.Services;
using Restaurant.ViewModels;
using Restaurant.Views;
using System.Windows;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register DbContext
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=RestaurantDb;Trusted_Connection=True;MultipleActiveResultSets=true",
                    b => b.MigrationsAssembly("Restaurant")));

            // Register repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register services
            services.AddSingleton<IUserService, UserService>();

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();

            // Register Views
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginView>();
            services.AddTransient<RegisterView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        public static ServiceProvider Services => ((App)Current).serviceProvider;
    }
}
