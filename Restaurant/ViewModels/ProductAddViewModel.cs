using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using Restaurant.Services.Interfaces;

namespace Restaurant.ViewModels
{
    public class ProductAddViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IAllergenService _allergenService;
        private readonly INavigationService _navigationService;

        private string _name;
        private string _description;
        private int? _selectedCategoryId;
        private decimal _price;
        private int _portionQuantity;
        private int _totalQuantity;
        private bool _isAvailable;
        private int _prepTime;
        private string _errorMessage;
        private bool _isLoading;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public int? SelectedCategoryId
        {
            get => _selectedCategoryId;
            set => SetProperty(ref _selectedCategoryId, value);
        }

        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public int PortionQuantity
        {
            get => _portionQuantity;
            set => SetProperty(ref _portionQuantity, value);
        }

        public int TotalQuantity
        {
            get => _totalQuantity;
            set => SetProperty(ref _totalQuantity, value);
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }

        public int PrepTime
        {
            get => _prepTime;
            set => SetProperty(ref _prepTime, value);
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

        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
        public ObservableCollection<Allergen> Allergens { get; } = new ObservableCollection<Allergen>();
        public ObservableCollection<Allergen> SelectedAllergens { get; } = new ObservableCollection<Allergen>();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddAllergenCommand { get; }
        public ICommand RemoveAllergenCommand { get; }

        public ProductAddViewModel(
            IProductService productService,
            ICategoryService categoryService,
            IAllergenService allergenService,
            INavigationService navigationService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _allergenService = allergenService;
            _navigationService = navigationService;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(() => _navigationService.NavigateToProductList());
            AddAllergenCommand = new RelayCommand<Allergen>(AddAllergen);
            RemoveAllergenCommand = new RelayCommand<Allergen>(RemoveAllergen);

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var categories = await _categoryService.GetAllCategoriesAsync();
                var allergens = await _allergenService.GetAllAllergensAsync();

                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                Allergens.Clear();
                foreach (var allergen in allergens)
                {
                    Allergens.Add(allergen);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                if (string.IsNullOrWhiteSpace(Name))
                {
                    ErrorMessage = "Name is required";
                    return;
                }

                if (!SelectedCategoryId.HasValue)
                {
                    ErrorMessage = "Category is required";
                    return;
                }

                var product = new Product
                {
                    Name = Name,
                    Description = Description,
                    CategoryId = SelectedCategoryId.Value,
                    Price = Price,
                    PortionQuantity = PortionQuantity,
                    TotalQuantity = TotalQuantity,
                    IsAvailable = IsAvailable,
                    PrepTime = PrepTime
                };

                var createdProduct = await _productService.CreateProductAsync(product);
                var allergenIds = SelectedAllergens.Select(a => a.Id).ToList();
                await _productService.UpdateProductAllergensAsync(createdProduct.Id, allergenIds);

                _navigationService.NavigateToProductList();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error saving product: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddAllergen(Allergen allergen)
        {
            if (allergen == null) return;

            if (!SelectedAllergens.Contains(allergen))
            {
                SelectedAllergens.Add(allergen);
            }
        }

        private void RemoveAllergen(Allergen allergen)
        {
            if (allergen == null) return;

            SelectedAllergens.Remove(allergen);
        }
    }
} 