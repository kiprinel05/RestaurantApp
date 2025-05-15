using Restaurant.Commands;
using Restaurant.Data;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Dish> _dishes;
        private string _searchText;
        private Category _selectedCategory;
        private bool _isLoading;
        private bool _isAuthenticated;
        private string _userDisplayName;

        public MainViewModel(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            Categories = new ObservableCollection<Category>();
            Dishes = new ObservableCollection<Dish>();

            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            SearchCommand = new RelayCommand(async () => await SearchDishesAsync());
            FilterByCategoryCommand = new RelayCommand(async () => await FilterDishesByCategoryAsync());
            LogoutCommand = new RelayCommand(Logout);

            _userService.UserChanged += UserService_UserChanged;
            UpdateAuthenticationState();

            // Load initial data
            _ = LoadDataAsync();
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    _ = FilterDishesByCategoryAsync();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => SetProperty(ref _isAuthenticated, value);
        }

        public string UserDisplayName
        {
            get => _userDisplayName;
            set => SetProperty(ref _userDisplayName, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand LogoutCommand { get; }

        private void UserService_UserChanged(object sender, EventArgs e)
        {
            UpdateAuthenticationState();
        }

        private void UpdateAuthenticationState()
        {
            IsAuthenticated = _userService.IsAuthenticated;
            if (IsAuthenticated)
            {
                var user = _userService.CurrentUser;
                UserDisplayName = $"{user.FirstName} {user.LastName}";
            }
            else
            {
                UserDisplayName = string.Empty;
            }
        }

        private void Logout()
        {
            _userService.ClearCurrentUser();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var dishes = await _unitOfWork.Dishes.GetDishesWithDetailsAsync();

                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                Dishes.Clear();
                foreach (var dish in dishes)
                {
                    Dishes.Add(dish);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchDishesAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadDataAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var dishes = await _unitOfWork.Dishes.SearchDishesAsync(SearchText);
                Dishes.Clear();
                foreach (var dish in dishes)
                {
                    Dishes.Add(dish);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FilterDishesByCategoryAsync()
        {
            if (SelectedCategory == null)
            {
                await LoadDataAsync();
                return;
            }

            IsLoading = true;
            try
            {
                var dishes = await _unitOfWork.Dishes.GetDishesByCategoryAsync(SelectedCategory.Id);
                Dishes.Clear();
                foreach (var dish in dishes)
                {
                    Dishes.Add(dish);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
} 