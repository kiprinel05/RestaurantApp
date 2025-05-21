using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Services;
using Restaurant.Views;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class EmployeeDashboardViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authService;

        public ICommand ManageCategoriesCommand { get; }
        public ICommand ManageProductsCommand { get; }
        public ICommand ManageMenusCommand { get; }
        public ICommand ViewOrdersCommand { get; }
        public ICommand ViewActiveOrdersCommand { get; }
        public ICommand ViewLowStockCommand { get; }
        public ICommand LogoutCommand { get; }

        [ObservableProperty]
        private string welcomeMessage;

        public EmployeeDashboardViewModel(INavigationService navigationService, IAuthenticationService authService)
        {
            _navigationService = navigationService;
            _authService = authService;

            // Initialize commands
            ManageCategoriesCommand = new RelayCommand(ManageCategories);
            ManageProductsCommand = new RelayCommand(ManageProducts);
            ManageMenusCommand = new RelayCommand(ManageMenus);
            ViewOrdersCommand = new RelayCommand(ViewOrders);
            ViewActiveOrdersCommand = new RelayCommand(ViewActiveOrders);
            ViewLowStockCommand = new RelayCommand(ViewLowStock);
            LogoutCommand = new RelayCommand(Logout);

            // Set welcome message
            var currentUser = _authService.GetCurrentUser();
            WelcomeMessage = $"Welcome, {currentUser?.FirstName} {currentUser?.LastName}!";
        }

        private void ManageCategories()
        {
            _navigationService.NavigateToCategoryList();
        }

        private void ManageProducts()
        {
            _navigationService.NavigateToProductList();
        }

        private void ManageMenus()
        {
            _navigationService.NavigateToMenuList();
        }

        private void ViewOrders()
        {
            // Will implement navigation to orders view
        }

        private void ViewActiveOrders()
        {
            // Will implement navigation to active orders view
        }

        private void ViewLowStock()
        {
            // Will implement navigation to low stock items view
        }

        private void Logout()
        {
            _authService.Logout();
            _navigationService.NavigateToAuth();
        }
    }
} 