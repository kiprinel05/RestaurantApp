using Microsoft.Extensions.DependencyInjection;
using Restaurant.Models;
using Restaurant.ViewModels;
using Restaurant.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainWindow _mainWindow;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationService _authService;
        private Frame NavigationFrame => _mainWindow.MainFrame;

        public NavigationService(Window mainWindow, IServiceProvider serviceProvider, IAuthenticationService authService)
        {
            _mainWindow = mainWindow as MainWindow ?? throw new ArgumentException("Window must be MainWindow", nameof(mainWindow));
            _serviceProvider = serviceProvider;
            _authService = authService;
        }

        public void NavigateToAuth()
        {
            var authViewModel = _serviceProvider.GetRequiredService<AuthViewModel>();
            var authView = new AuthView(authViewModel);
            NavigationFrame.Navigate(authView);
        }

        public void NavigateToMain()
        {
            var currentUser = _authService.GetCurrentUser();
            
            if (currentUser?.Role == UserRole.Employee)
            {
                var dashboardViewModel = _serviceProvider.GetRequiredService<EmployeeDashboardViewModel>();
                var dashboardView = new EmployeeDashboardView(dashboardViewModel);
                NavigationFrame.Navigate(dashboardView);
            }
            else
            {
                NavigateToMenu(); // For customers and guests
            }
        }

        public void NavigateToMenu()
        {
            var menuListViewModel = _serviceProvider.GetRequiredService<MenuListViewModel>();
            var menuView = new MenuView { DataContext = menuListViewModel };
            NavigationFrame.Navigate(menuView);
        }

        public void NavigateTo(Type viewType)
        {
            NavigateTo(viewType, null);
        }

        public void NavigateTo(Type viewType, object? parameter)
        {
            if (viewType == typeof(CategoryEditView))
            {
                var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
                var viewModel = new CategoryEditViewModel(categoryService, this, parameter as int?);
                var view = new CategoryEditView { DataContext = viewModel };
                NavigationFrame.Navigate(view);
            }
            else if (viewType == typeof(CategoryListView))
            {
                var viewModel = ActivatorUtilities.CreateInstance<CategoryListViewModel>(
                    _serviceProvider,
                    _serviceProvider.GetRequiredService<ICategoryService>(),
                    this);

                var view = new CategoryListView(viewModel);
                NavigationFrame.Navigate(view);
            }
            else
            {
                throw new NotImplementedException($"Navigation to {viewType.Name} is not implemented.");
            }
        }

        public void NavigateBack()
        {
            if (NavigationFrame.CanGoBack)
            {
                NavigationFrame.GoBack();
            }
        }
    }
} 