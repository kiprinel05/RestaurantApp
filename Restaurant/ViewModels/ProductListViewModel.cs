using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class ProductListViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<Product> products = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand RefreshCommand { get; }

        public ProductListViewModel(
            IProductService productService,
            INavigationService navigationService)
        {
            _productService = productService;
            _navigationService = navigationService;

            AddProductCommand = new RelayCommand(AddProduct);
            EditProductCommand = new RelayCommand<Product>(EditProduct);
            DeleteProductCommand = new AsyncRelayCommand<Product>(DeleteProductAsync);
            RefreshCommand = new AsyncRelayCommand(LoadProductsAsync);

            LoadProductsAsync().ConfigureAwait(false);
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var productList = await _productService.GetAllProductsAsync();
                Products = new ObservableCollection<Product>(productList);
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Failed to load products: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddProduct()
        {
            _navigationService.NavigateToProductEdit(null);
        }

        private void EditProduct(Product? product)
        {
            if (product != null)
            {
                _navigationService.NavigateToProductEdit(product.Id);
            }
        }

        private async Task DeleteProductAsync(Product? product)
        {
            if (product == null) return;

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _productService.DeleteProductAsync(product.Id);
                await LoadProductsAsync();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Failed to delete product: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
} 