using Microsoft.Extensions.DependencyInjection;
using Restaurant.ViewModels;
using Restaurant.Views;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainWindow _mainWindow;
        private readonly IServiceProvider _serviceProvider;
        private Frame NavigationFrame => _mainWindow.MainFrame;

        public NavigationService(Window mainWindow, IServiceProvider serviceProvider)
        {
            _mainWindow = mainWindow as MainWindow ?? throw new ArgumentException("Window must be MainWindow", nameof(mainWindow));
            _serviceProvider = serviceProvider;
        }

        public void NavigateToAuth()
        {
            var authViewModel = _serviceProvider.GetRequiredService<AuthViewModel>();
            var authView = new AuthView(authViewModel);
            NavigationFrame.Navigate(authView);
        }

        public void NavigateToMain()
        {
            NavigateToMenu(); // After authentication, go directly to menu
        }

        public void NavigateToMenu()
        {
            var menuListViewModel = _serviceProvider.GetRequiredService<MenuListViewModel>();
            var menuView = new MenuView { DataContext = menuListViewModel };
            NavigationFrame.Navigate(menuView);
        }

        public void NavigateTo(System.Type viewType)
        {
            // This method is not used anymore, but kept for interface compatibility
            throw new System.NotImplementedException();
        }
    }
} 