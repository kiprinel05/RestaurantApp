using Restaurant.ViewModels;
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

            // Attach event handlers for PasswordBoxes
            Loaded += (s, e) =>
            {
                // Ensure we start with login form
                _viewModel.ShowLogin();

                // Attach password handlers
                LoginPasswordBox.PasswordChanged += (s, e) => _viewModel.LoginPassword = LoginPasswordBox.Password;
                RegisterPasswordBox.PasswordChanged += (s, e) => _viewModel.RegisterPassword = RegisterPasswordBox.Password;
                ConfirmPasswordBox.PasswordChanged += (s, e) => _viewModel.ConfirmPassword = ConfirmPasswordBox.Password;

                // Clear passwords when visibility changes
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
    }
} 