using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        private readonly ICategoryService _categoryService;
        private readonly INavigationService _navigationService;

        private List<Category> _allCategories = new();

        [ObservableProperty]
        private ObservableCollection<Category> categories;

        [ObservableProperty]
        private Category selectedCategory;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private string selectedSortOption;

        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "Nume (A-Z)",
            "Nume (Z-A)",
            "Cele mai noi",
            "Cele mai vechi"
        };

        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoriesViewModel(ICategoryService categoryService, INavigationService navigationService)
        {
            _categoryService = categoryService;
            _navigationService = navigationService;

            Categories = new ObservableCollection<Category>();
            SelectedSortOption = SortOptions[0];

            AddCategoryCommand = new RelayCommand(AddCategory);
            EditCategoryCommand = new RelayCommand<Category>(EditCategory);
            DeleteCategoryCommand = new RelayCommand<Category>(DeleteCategory);

            LoadCategories();
        }

        private async void LoadCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            _allCategories = categories.ToList();
            ApplySearchAndSorting();
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplySearchAndSorting();
        }

        partial void OnSelectedSortOptionChanged(string value)
        {
            ApplySearchAndSorting();
        }

        private void ApplySearchAndSorting()
        {
            IEnumerable<Category> filtered = _allCategories;
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var lower = SearchText.ToLower();
                filtered = filtered.Where(c =>
                    (c.Name != null && c.Name.ToLower().Contains(lower)) ||
                    (c.Description != null && c.Description.ToLower().Contains(lower)));
            }

            filtered = SelectedSortOption switch
            {
                "Nume (A-Z)" => filtered.OrderBy(c => c.Name),
                "Nume (Z-A)" => filtered.OrderByDescending(c => c.Name),
                "Cele mai noi" => filtered.OrderByDescending(c => c.Id),
                "Cele mai vechi" => filtered.OrderBy(c => c.Id),
                _ => filtered.OrderBy(c => c.Name)
            };

            Categories.Clear();
            foreach (var category in filtered)
            {
                Categories.Add(category);
            }
        }

        private void AddCategory()
        {
            _navigationService.NavigateToCategoryEdit(null);
        }

        private void EditCategory(Category category)
        {
            if (category != null)
            {
                _navigationService.NavigateToCategoryEdit(category.Id);
            }
        }

        private async void DeleteCategory(Category category)
        {
            if (category == null) return;

            var result = MessageBox.Show(
                "Sigur doriți să ștergeți această categorie?",
                "Confirmare ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _categoryService.DeleteCategoryAsync(category.Id);
                    Categories.Remove(category);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(
                        $"Eroare la ștergerea categoriei: {ex.Message}",
                        "Eroare",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
} 