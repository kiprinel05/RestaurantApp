using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using Restaurant.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class CategoryListViewModel : ObservableObject
    {
        private readonly ICategoryService _categoryService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<Category> Categories { get; } = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand BackCommand { get; }

        public CategoryListViewModel(ICategoryService categoryService, INavigationService navigationService)
        {
            _categoryService = categoryService;
            _navigationService = navigationService;

            AddCategoryCommand = new AsyncRelayCommand(AddCategoryAsync);
            EditCategoryCommand = new AsyncRelayCommand<Category>(EditCategoryAsync);
            DeleteCategoryCommand = new AsyncRelayCommand<Category>(DeleteCategoryAsync);
            RefreshCommand = new AsyncRelayCommand(LoadCategoriesAsync);
            BackCommand = new RelayCommand(() => _navigationService.NavigateToMain());

            // Load categories when ViewModel is created
            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                Categories.Clear();

                var categories = await _categoryService.GetAllCategoriesAsync();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error loading categories: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddCategoryAsync()
        {
            _navigationService.NavigateTo(typeof(CategoryEditView), null);
        }

        private async Task EditCategoryAsync(Category? category)
        {
            if (category == null) return;
            _navigationService.NavigateTo(typeof(CategoryEditView), category.Id);
        }

        private async Task DeleteCategoryAsync(Category? category)
        {
            if (category == null) return;

            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete category '{category.Name}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes) return;

                await _categoryService.DeleteCategoryAsync(category.Id);
                Categories.Remove(category);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot Delete Category",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error deleting category: {ex.Message}";
            }
        }
    }
} 