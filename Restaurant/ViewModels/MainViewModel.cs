using Restaurant.Data;
using Restaurant.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System;

namespace Restaurant.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Dish> _dishes;
        private string _searchText;
        private Category _selectedCategory;
        private bool _isLoading;

        public MainViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Categories = new ObservableCollection<Category>();
            Dishes = new ObservableCollection<Dish>();
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            SearchCommand = new RelayCommand(async () => await SearchDishesAsync());
            FilterByCategoryCommand = new RelayCommand(async () => await FilterDishesByCategoryAsync());
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

        public ICommand LoadDataCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }

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