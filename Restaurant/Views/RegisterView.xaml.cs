using System.Windows;
using System.Windows.Input;
using Restaurant.ViewModels;

namespace Restaurant.Views
{
    public partial class RegisterView : Window
    {
        private readonly RegisterViewModel _viewModel;

        public RegisterView(RegisterViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Set up password changed handlers
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            ConfirmPasswordBox.PasswordChanged += ConfirmPasswordBox_PasswordChanged;

            // Set up window close handler
            _viewModel.CloseWindow = () => Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
        }

        private void Login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginView = App.Services.GetService<LoginView>();
            loginView.Show();
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
} 