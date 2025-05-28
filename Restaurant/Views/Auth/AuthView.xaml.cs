using Restaurant.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.Views
{
    public partial class AuthView : Page
    {
        private readonly AuthViewModel _viewModel;

        public AuthView(AuthViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            Loaded += (s, e) =>
            {
                _viewModel.ShowLogin();

                LoginPasswordBox.PasswordChanged += OnLoginPasswordChanged;
                RegisterPasswordBox.PasswordChanged += OnRegisterPasswordChanged;
                ConfirmPasswordBox.PasswordChanged += OnConfirmPasswordChanged;

                _viewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(AuthViewModel.IsLoginVisible))
                    {
                        LoginPasswordBox.Password = string.Empty;
                        RegisterPasswordBox.Password = string.Empty;
                        ConfirmPasswordBox.Password = string.Empty;
                    }
                };
            };
        }

        private void OnLoginPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.LoginPassword = passwordBox.Password;
            }
        }

        private void OnRegisterPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.RegisterPassword = passwordBox.Password;
            }
        }

        private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.ConfirmPassword = passwordBox.Password;
            }
        }

        private void OnShowRegisterClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowRegister();
        }

        private void OnShowLoginClick(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowLogin();
        }
    }
} 