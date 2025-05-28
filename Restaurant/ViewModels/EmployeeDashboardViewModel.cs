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

        public EmployeeDashboardViewModel(
            INavigationService navigationService, 
            IAuthenticationService authService,
            IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _authService = authService;
            _serviceProvider = serviceProvider;

            // Initialize commands
            NavigateToCategoriesCommand = new RelayCommand(NavigateToCategories);
            NavigateToProductsCommand = new RelayCommand(NavigateToProducts);
            NavigateToMenusCommand = new RelayCommand(NavigateToMenus);
            NavigateToAllergensCommand = new RelayCommand(NavigateToAllergens);
            NavigateToAllOrdersCommand = new RelayCommand(NavigateToAllOrders);
            NavigateToActiveOrdersCommand = new RelayCommand(NavigateToActiveOrders);
            NavigateToLowStockCommand = new RelayCommand(NavigateToLowStock);
            LogoutCommand = new RelayCommand(Logout);

            // Set welcome message
            var currentUser = _authService.GetCurrentUser();
            WelcomeMessage = $"Bine ai venit, {currentUser?.FirstName} {currentUser?.LastName}!";

            // Set default view (Categories)
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
            CurrentView = new ProductsView(viewModel);
        }

        private void NavigateToMenus()
        {
            // To be implemented
        }

        private void NavigateToAllergens()
        {
            // To be implemented
        }

        private void NavigateToAllOrders()
        {
            // To be implemented
        }

        private void NavigateToActiveOrders()
        {
            // To be implemented
        }

        private void NavigateToLowStock()
        {
            // To be implemented
        }

        private void Logout()
        {
            _authService.Logout();
            _navigationService.NavigateToAuth();
        }
    }
} 