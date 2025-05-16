using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Restaurant.Models;
using Restaurant.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class ProductEditViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly INavigationService _navigationService;
        private readonly ICategoryService _categoryService;
        private Product? _existingProduct;

        [ObservableProperty]
        private string title = "Add Product";

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private int portionQuantity;

        [ObservableProperty]
        private int totalQuantity;

        [ObservableProperty]
        private bool isAvailable;

        [ObservableProperty]
        private int prepTime;

        [ObservableProperty]
        private int selectedCategoryId;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();

        [ObservableProperty]
        private ObservableCollection<Allergen> allergens = new();

        [ObservableProperty]
        private ObservableCollection<Allergen> selectedAllergens = new();

        [ObservableProperty]
        private ObservableCollection<string> productImages = new();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddImageCommand { get; }
        public ICommand RemoveImageCommand { get; }
        public ICommand AddAllergenCommand { get; }
        public ICommand RemoveAllergenCommand { get; }

        public ProductEditViewModel(
            IProductService productService,
            INavigationService navigationService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _navigationService = navigationService;
            _categoryService = categoryService;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
            AddImageCommand = new AsyncRelayCommand(AddImageAsync);
            RemoveImageCommand = new AsyncRelayCommand<string>(RemoveImageAsync);
            AddAllergenCommand = new RelayCommand<Allergen>(AddAllergen);
            RemoveAllergenCommand = new RelayCommand<Allergen>(RemoveAllergen);

            LoadDataAsync().ConfigureAwait(false);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var categoriesTask = _categoryService.GetAllCategoriesAsync();
                var allergensTask = _productService.GetAllergensAsync();

                await Task.WhenAll(categoriesTask, allergensTask);

                Categories = new ObservableCollection<Category>(await categoriesTask);
                Allergens = new ObservableCollection<Allergen>(await allergensTask);

                if (Categories.Any())
                {
                    SelectedCategoryId = Categories.First().Id;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadProductAsync(int? productId)
        {
            if (!productId.HasValue)
            {
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                _existingProduct = await _productService.GetProductByIdAsync(productId.Value);
                if (_existingProduct == null)
                {
                    ErrorMessage = "Product not found.";
                    return;
                }

                Title = "Edit Product";
                Name = _existingProduct.Name;
                Description = _existingProduct.Description;
                Price = _existingProduct.Price;
                PortionQuantity = _existingProduct.PortionQuantity;
                TotalQuantity = _existingProduct.TotalQuantity;
                IsAvailable = _existingProduct.IsAvailable;
                PrepTime = _existingProduct.PrepTime;
                SelectedCategoryId = _existingProduct.CategoryId;

                SelectedAllergens = new ObservableCollection<Allergen>(_existingProduct.Allergens);
                ProductImages = new ObservableCollection<string>(_existingProduct.Images.Select(i => i.ImagePath));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load product: {ex.Message}";
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
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Name))
                {
                    ErrorMessage = "Name is required.";
                    return;
                }

                if (Price <= 0)
                {
                    ErrorMessage = "Price must be greater than 0.";
                    return;
                }

                var product = _existingProduct ?? new Product();
                product.Name = Name;
                product.Description = Description;
                product.Price = Price;
                product.PortionQuantity = PortionQuantity;
                product.TotalQuantity = TotalQuantity;
                product.IsAvailable = IsAvailable;
                product.PrepTime = PrepTime;
                product.CategoryId = SelectedCategoryId;

                if (_existingProduct == null)
                {
                    await _productService.CreateProductAsync(product);
                }
                else
                {
                    await _productService.UpdateProductAsync(product);
                }

                await _productService.UpdateProductAllergensAsync(
                    product.Id, 
                    SelectedAllergens.Select(a => a.Id).ToList());

                MessageBox.Show(
                    _existingProduct == null ? "Product created successfully!" : "Product updated successfully!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                _navigationService.NavigateBack();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to save product: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Cancel()
        {
            _navigationService.NavigateBack();
        }

        private async Task AddImageAsync()
        {
            if (_existingProduct == null)
            {
                ErrorMessage = "Please save the product before adding images.";
                return;
            }

            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    IsLoading = true;
                    ErrorMessage = string.Empty;

                    var imageBytes = await File.ReadAllBytesAsync(dialog.FileName);
                    var base64Image = Convert.ToBase64String(imageBytes);
                    var fileName = Path.GetFileName(dialog.FileName);

                    var imagePath = await _productService.SaveProductImageAsync(
                        _existingProduct.Id,
                        base64Image,
                        fileName);

                    ProductImages.Add(imagePath);
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Failed to add image: {ex.Message}";
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private async Task RemoveImageAsync(string? imagePath)
        {
            if (_existingProduct == null || string.IsNullOrEmpty(imagePath))
            {
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _productService.DeleteProductImageAsync(_existingProduct.Id, imagePath);
                ProductImages.Remove(imagePath);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to remove image: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddAllergen(Allergen? allergen)
        {
            if (allergen == null || SelectedAllergens.Any(a => a.Id == allergen.Id))
            {
                return;
            }

            SelectedAllergens.Add(allergen);
        }

        private void RemoveAllergen(Allergen? allergen)
        {
            if (allergen == null)
            {
                return;
            }

            var existingAllergen = SelectedAllergens.FirstOrDefault(a => a.Id == allergen.Id);
            if (existingAllergen != null)
            {
                SelectedAllergens.Remove(existingAllergen);
            }
        }
    }
} 