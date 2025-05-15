using Restaurant.Commands;
using Restaurant.Data;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _deliveryAddress;
        private string _password;
        private string _confirmPassword;
        private string _errorMessage;
        private bool _isLoading;

        public RegisterViewModel(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            RegisterCommand = new RelayCommand(async () => await RegisterAsync(), CanRegister);
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (SetProperty(ref _phoneNumber, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set
            {
                if (SetProperty(ref _deliveryAddress, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
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
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
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

        public ICommand RegisterCommand { get; }

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   !string.IsNullOrWhiteSpace(DeliveryAddress) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   Password == ConfirmPassword &&
                   !IsLoading;
        }

        private async Task RegisterAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Check if email already exists
                var existingUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == Email);
                if (existingUser != null)
                {
                    ErrorMessage = "Email already registered";
                    return;
                }

                // Create new user
                var user = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    PhoneNumber = PhoneNumber,
                    DeliveryAddress = DeliveryAddress,
                    PasswordHash = HashPassword(Password),
                    Role = UserRole.Customer
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                _userService.SetCurrentUser(user);
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during registration. Please try again.";
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