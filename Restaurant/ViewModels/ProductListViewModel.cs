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

        public ICommand AddProductCommand { get; set; }
        public ICommand EditProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; }
        public ICommand RefreshCommand { get; }

        public ProductListViewModel(
            IProductService productService,
            INavigationService navigationService)
        {
            _productService = productService;
            _navigationService = navigationService;

            RefreshCommand = new AsyncRelayCommand(LoadProductsAsync);
            AddProductCommand = new RelayCommand(() => _navigationService.NavigateToProductAdd());
            EditProductCommand = new RelayCommand<int>(id => _navigationService.NavigateToProductEdit(id));
            DeleteProductCommand = new AsyncRelayCommand<int>(async id => await DeleteProductAsync(id));

            LoadProductsAsync();
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

        private async Task DeleteProductAsync(int id)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _productService.DeleteProductAsync(id);
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