using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class CategoryEditViewModel : ObservableObject, Restaurant.Services.INavigationAware
    {
        private readonly ICategoryService _categoryService;
        private readonly INavigationService _navigationService;
        private int? _categoryId;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        public string Title => _categoryId.HasValue ? "Edit Category" : "Add Category";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CategoryEditViewModel(
            ICategoryService categoryService,
            INavigationService navigationService)
        {
            _categoryService = categoryService;
            _navigationService = navigationService;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(() => _navigationService.NavigateBack());
        }

        public async Task LoadCategoryAsync(int categoryId)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var category = await _categoryService.GetCategoryByIdAsync(categoryId);
                if (category != null)
                {
                    _categoryId = category.Id;
                    Name = category.Name;
                    Description = category.Description;
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error loading category: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Name is required";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var isUnique = await _categoryService.IsCategoryNameUniqueAsync(Name, _categoryId);
                if (!isUnique)
                {
                    ErrorMessage = "A category with this name already exists";
                    return;
                }

                var category = new Category
                {
                    Id = _categoryId ?? 0,
                    Name = Name,
                    Description = Description
                };

                if (_categoryId.HasValue)
                {
                    await _categoryService.UpdateCategoryAsync(category);
                }
                else
                {
                    await _categoryService.AddCategoryAsync(category);
                }

                _navigationService.NavigateBack();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error saving category: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is int categoryId)
            {
                _ = LoadCategoryAsync(categoryId);
            }
            else
            {
                _categoryId = null;
                Name = string.Empty;
                Description = string.Empty;
            }
        }
    }
} 