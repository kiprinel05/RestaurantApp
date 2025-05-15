using System.Windows;
using System.Windows.Input;
using Restaurant.ViewModels;

namespace Restaurant.Views
{
    public partial class LoginView : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Set up password changed handler
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;

            // Set up window close handler
            _viewModel.CloseWindow = () => Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
        }

        private void Register_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registerView = App.Services.GetService<RegisterView>();
            registerView.Show();
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
} 