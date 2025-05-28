using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Services;
using Restaurant.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class EmployeeDashboardViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authService;

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

        public EmployeeDashboardViewModel(INavigationService navigationService, IAuthenticationService authService)
        {
            _navigationService = navigationService;
            _authService = authService;

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

            // Set default view (to be implemented)
            CurrentView = new UserControl(); // Placeholder
        }

        private void NavigateToCategories()
        {
            // To be implemented
        }

        private void NavigateToProducts()
        {
            // To be implemented
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