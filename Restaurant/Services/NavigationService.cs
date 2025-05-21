using Microsoft.Extensions.DependencyInjection;
using Restaurant.Models;
using Restaurant.ViewModels;
using Restaurant.Views;
using Restaurant.Views.Product;
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

        public void NavigateTo(Type viewType)
        {
            var view = _serviceProvider.GetRequiredService(viewType) as Page;
            if (view != null)
            {
                NavigateTo(view);
            }
        }

        public void NavigateTo(Type viewType, object parameter)
        {
            var view = _serviceProvider.GetRequiredService(viewType) as Page;
            if (view != null && view.DataContext is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(parameter);
                NavigateTo(view);
            }
        }

        public void NavigateToAuth()
        {
            var viewModel = _serviceProvider.GetRequiredService<AuthViewModel>();
            var view = _serviceProvider.GetRequiredService<AuthView>();
            NavigateTo(view);
        }

        public void NavigateToMain()
        {
            var currentUser = _authService.GetCurrentUser();
            
            if (currentUser?.Role == UserRole.Employee)
            {
                var viewModel = _serviceProvider.GetRequiredService<EmployeeDashboardViewModel>();
                var view = _serviceProvider.GetRequiredService<EmployeeDashboardView>();
                NavigateTo(view);
            }
            else
            {
                NavigateToMenu(); // For customers and guests
            }
        }

        public void NavigateToMenu()
        {
            var viewModel = _serviceProvider.GetRequiredService<MenuListViewModel>();
            var view = _serviceProvider.GetRequiredService<MenuView>();
            NavigateTo(view);
        }

        public void NavigateToEmployeeDashboard()
        {
            if (!_authService.IsUserInRole("Employee"))
            {
                MessageBox.Show("Access denied. Employee role required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var viewModel = _serviceProvider.GetRequiredService<EmployeeDashboardViewModel>();
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

            var viewModel = _serviceProvider.GetRequiredService<CategoryListViewModel>();
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

            var viewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
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
                viewModel.OnNavigatedTo(productId.Value);
            }
            else
            {
                viewModel.OnNavigatedTo(null);
            }

            NavigateTo(view);
        }

        public void NavigateToProductAdd()
        {
            var view = new ProductAddView();
            var viewModel = _serviceProvider.GetRequiredService<ProductAddViewModel>();
            view.DataContext = viewModel;
            NavigateTo(view);
        }

        public void NavigateBack()
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop(); // Remove current page
                var previousPage = _navigationStack.Peek();
                _mainWindow.MainFrame.Navigate(previousPage);
            }
        }

        private void NavigateTo(Page view)
        {
            _navigationStack.Push(view);
            _mainWindow.MainFrame.Navigate(view);
        }
    }

    public interface INavigationAware
    {
        void OnNavigatedTo(object parameter);
    }
} 