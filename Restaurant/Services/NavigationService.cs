using Microsoft.Extensions.DependencyInjection;
using Restaurant.Models;
using Restaurant.ViewModels;
using Restaurant.Views;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.Services
{
    public class NavigationService : INavigationService
    {
        private readonly MainWindow _mainWindow;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationService _authService;
        private readonly Stack<Page> _navigationStack = new();

        public Page? CurrentPage => _navigationStack.Count > 0 ? _navigationStack.Peek() : null;

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
            NavigateTo(authView);
        }

        public void NavigateToMain()
        {
            var currentUser = _authService.GetCurrentUser();
            
            if (currentUser?.Role == UserRole.Employee)
            {
                var dashboardViewModel = _serviceProvider.GetRequiredService<EmployeeDashboardViewModel>();
                var dashboardView = new EmployeeDashboardView(dashboardViewModel);
                NavigateTo(dashboardView);
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
            NavigateTo(menuView);
        }

        public void NavigateToEmployeeDashboard()
        {
            if (!_authService.IsUserInRole("Employee"))
            {
                MessageBox.Show("Access denied. Employee role required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var view = _serviceProvider.GetRequiredService<EmployeeDashboardView>();
            NavigateTo(view);
        }

        public void NavigateToCategoryList()
        {
            if (!_authService.IsUserInRole("Employee"))
            {
                MessageBox.Show("Access denied. Employee role required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var view = _serviceProvider.GetRequiredService<CategoryListView>();
            NavigateTo(view);
        }

        public void NavigateToCategoryEdit(int? categoryId)
        {
            if (!_authService.IsUserInRole("Employee"))
            {
                MessageBox.Show("Access denied. Employee role required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var viewModel = _serviceProvider.GetRequiredService<CategoryEditViewModel>();
            var view = _serviceProvider.GetRequiredService<CategoryEditView>();

            if (categoryId.HasValue)
            {
                viewModel.LoadCategoryAsync(categoryId.Value).ConfigureAwait(false);
            }

            NavigateTo(view);
        }

        public void NavigateToProductList()
        {
            if (!_authService.IsUserInRole("Employee"))
            {
                MessageBox.Show("Access denied. Employee role required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var view = _serviceProvider.GetRequiredService<ProductListView>();
            NavigateTo(view);
        }

        public void NavigateToProductEdit(int? productId)
        {
            if (!_authService.IsUserInRole("Employee"))
            {
                MessageBox.Show("Access denied. Employee role required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var viewModel = _serviceProvider.GetRequiredService<ProductEditViewModel>();
            var view = _serviceProvider.GetRequiredService<ProductEditView>();

            if (productId.HasValue)
            {
                viewModel.LoadProductAsync(productId.Value).ConfigureAwait(false);
            }

            NavigateTo(view);
        }

        public void NavigateBack()
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop(); // Remove current page
                var previousPage = _navigationStack.Peek();
                (_mainWindow as MainWindow)?.NavigationFrame.Navigate(previousPage);
            }
        }

        private void NavigateTo(Page view)
        {
            _navigationStack.Push(view);
            (_mainWindow as MainWindow)?.NavigationFrame.Navigate(view);
        }
    }
} 