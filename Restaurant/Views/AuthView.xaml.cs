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

            // Atașăm event handlers pentru PasswordBox-uri
            Loaded += (s, e) =>
            {
                // Asigurăm-ne că începem cu formularul de login
                _viewModel.ShowLogin();

                // Atașăm event handlers pentru parole
                LoginPasswordBox.PasswordChanged += (s, e) => _viewModel.LoginPassword = LoginPasswordBox.Password;
                RegisterPasswordBox.PasswordChanged += (s, e) => _viewModel.RegisterPassword = RegisterPasswordBox.Password;
                ConfirmPasswordBox.PasswordChanged += (s, e) => _viewModel.ConfirmPassword = ConfirmPasswordBox.Password;

                // Curățăm parolele când se schimbă vizibilitatea
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