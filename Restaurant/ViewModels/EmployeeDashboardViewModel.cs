using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Services;
using Restaurant.Views;
using Restaurant.Views.Employee;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.ViewModels
{
    public partial class EmployeeDashboardViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string welcomeMessage;

        [ObservableProperty]
        private UserControl currentView;

        public ICommand NavigateToCategoriesCommand { get; }
        public ICommand NavigateToProductsCommand { get; }
        public ICommand NavigateToMenusCommand { get; }
        public ICommand NavigateToAllergensCommand { get; }
        public ICommand NavigateToAllOrdersCommand { get; }
        public ICommand NavigateToActiveOrdersCommand { get; }
        public ICommand NavigateToLowStockCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand NavigateToAddProductCommand { get; }
        public ICommand NavigateToEditProductCommand { get; }

        public EmployeeDashboardViewModel(
            INavigationService navigationService, 
            IAuthenticationService authService,
            IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _authService = authService;
            _serviceProvider = serviceProvider;

            NavigateToCategoriesCommand = new RelayCommand(NavigateToCategories);
            NavigateToProductsCommand = new RelayCommand(NavigateToProducts);
            NavigateToMenusCommand = new RelayCommand(NavigateToMenus);
            NavigateToAllergensCommand = new RelayCommand(NavigateToAllergens);
            NavigateToAllOrdersCommand = new RelayCommand(NavigateToAllOrders);
            NavigateToActiveOrdersCommand = new RelayCommand(NavigateToActiveOrders);
            NavigateToLowStockCommand = new RelayCommand(NavigateToLowStock);
            LogoutCommand = new RelayCommand(Logout);
            NavigateToAddProductCommand = new RelayCommand(NavigateToAddProduct);
            NavigateToEditProductCommand = new RelayCommand<int>(NavigateToEditProduct);

            var currentUser = _authService.GetCurrentUser();
            WelcomeMessage = $"Welcome, {currentUser?.FirstName} {currentUser?.LastName}!";

            // set default view
            NavigateToCategories();
        }

        private void NavigateToCategories()
        {
            var viewModel = _serviceProvider.GetRequiredService<CategoriesViewModel>();
            CurrentView = new CategoriesView(viewModel);
        }

        private void NavigateToProducts()
        {
            var viewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
            viewModel.AddProductCommand = NavigateToAddProductCommand;
            viewModel.EditProductCommand = NavigateToEditProductCommand;
            CurrentView = new ProductsView(viewModel);
        }

        private void NavigateToMenus()
        {
            // to be implemented
        }

        private void NavigateToAllergens()
        {
            // to be implemented
        }

        private void NavigateToAllOrders()
        {
            var viewModel = _serviceProvider.GetRequiredService<OrdersViewModel>();
            var view = new Restaurant.Views.Menu.OrdersView();
            view.DataContext = viewModel;
            CurrentView = view;
        }

        private void NavigateToActiveOrders()
        {
            var viewModel = _serviceProvider.GetRequiredService<OrdersViewModel>();
            viewModel.ShowActiveOrders = true;
            var view = new Restaurant.Views.Menu.OrdersView();
            view.DataContext = viewModel;
            CurrentView = view;
        }

        private void NavigateToLowStock()
        {
            var viewModel = _serviceProvider.GetRequiredService<LowStockViewModel>();
            CurrentView = new LowStockView();
            CurrentView.DataContext = viewModel;
        }

        private void Logout()
        {
            _authService.Logout();
            _navigationService.NavigateToAuth();
        }

        private void NavigateToAddProduct()
        {
            var viewModel = _serviceProvider.GetRequiredService<ProductEditViewModel>();
            viewModel.OnNavigatedTo(null);
            CurrentView = new ProductEditView(viewModel);
        }

        private void NavigateToEditProduct(int productId)
        {
            var viewModel = _serviceProvider.GetRequiredService<ProductEditViewModel>();
            viewModel.OnNavigatedTo(productId);
            CurrentView = new ProductEditView(viewModel);
        }
    }
} 