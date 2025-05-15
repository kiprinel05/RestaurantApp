using Restaurant.Commands;
using Restaurant.Data;
using Restaurant.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public LoginViewModel(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            LoginCommand = new RelayCommand(async () => await LoginAsync(), CanLogin);
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoginCommand { get; }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) && 
                   !string.IsNullOrWhiteSpace(Password) && 
                   !IsLoading;
        }

        private async Task LoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var hashedPassword = HashPassword(Password);
                var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => 
                    u.Email == Email && u.PasswordHash == hashedPassword);

                if (user == null)
                {
                    ErrorMessage = "Invalid email or password";
                    return;
                }

                _userService.SetCurrentUser(user);
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during login. Please try again.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Event to close the window
        public Action CloseWindow { get; set; }
    }
} 