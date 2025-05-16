using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Services;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private bool isLoginVisible = true;

        // Login properties
        [ObservableProperty]
        private string loginEmail = string.Empty;

        [ObservableProperty]
        private string loginPassword = string.Empty;

        [ObservableProperty]
        private string loginErrorMessage = string.Empty;

        // Register properties
        [ObservableProperty]
        private string firstName = string.Empty;

        [ObservableProperty]
        private string lastName = string.Empty;

        [ObservableProperty]
        private string registerEmail = string.Empty;

        [ObservableProperty]
        private string registerPassword = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private string phoneNumber = string.Empty;

        [ObservableProperty]
        private string deliveryAddress = string.Empty;

        [ObservableProperty]
        private string registerErrorMessage = string.Empty;

        public AuthViewModel(IAuthenticationService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
            IsLoginVisible = true;
        }

        [RelayCommand]
        public void ShowLogin()
        {
            IsLoginVisible = true;
            ClearErrors();
            ClearFields();
        }

        [RelayCommand]
        private void ShowRegister()
        {
            IsLoginVisible = false;
            ClearErrors();
            ClearFields();
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LoginEmail) || string.IsNullOrWhiteSpace(LoginPassword))
                {
                    LoginErrorMessage = "Please fill in all fields";
                    return;
                }

                var user = await _authService.LoginAsync(LoginEmail, LoginPassword);
                if (user == null)
                {
                    LoginErrorMessage = "Invalid email or password";
                    return;
                }

                _navigationService.NavigateToMain();
            }
            catch (System.Exception ex)
            {
                LoginErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
                    string.IsNullOrWhiteSpace(RegisterEmail) || string.IsNullOrWhiteSpace(RegisterPassword) ||
                    string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(PhoneNumber) ||
                    string.IsNullOrWhiteSpace(DeliveryAddress))
                {
                    RegisterErrorMessage = "Please fill in all fields";
                    return;
                }

                if (RegisterPassword != ConfirmPassword)
                {
                    RegisterErrorMessage = "Passwords do not match";
                    return;
                }

                var (success, message) = await _authService.RegisterAsync(
                    FirstName, LastName, RegisterEmail, RegisterPassword, PhoneNumber, DeliveryAddress);

                if (!success)
                {
                    RegisterErrorMessage = message;
                    return;
                }

                MessageBox.Show("Registration successful! Please login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowLogin();
            }
            catch (System.Exception ex)
            {
                RegisterErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private void ClearErrors()
        {
            LoginErrorMessage = string.Empty;
            RegisterErrorMessage = string.Empty;
        }

        private void ClearFields()
        {
            if (IsLoginVisible)
            {
                LoginEmail = string.Empty;
                LoginPassword = string.Empty;
            }
            else
            {
                FirstName = string.Empty;
                LastName = string.Empty;
                RegisterEmail = string.Empty;
                RegisterPassword = string.Empty;
                ConfirmPassword = string.Empty;
                PhoneNumber = string.Empty;
                DeliveryAddress = string.Empty;
            }
        }
    }
} 