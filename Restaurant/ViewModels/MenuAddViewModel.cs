using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class MenuAddViewModel : ObservableObject
    {
        private readonly IMenuService _menuService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string description;
        [ObservableProperty]
        private int? selectedCategoryId;
        [ObservableProperty]
        private ObservableCollection<Category> categories = new();
        [ObservableProperty]
        private ObservableCollection<Product> products = new();
        [ObservableProperty]
        private Product selectedProduct;
        [ObservableProperty]
        private ObservableCollection<Product> selectedProducts = new();
        [ObservableProperty]
        private Product selectedProductInMenu;

        public ICommand AddProductToMenuCommand { get; }
        public ICommand RemoveProductFromMenuCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public MenuAddViewModel(IMenuService menuService, ICategoryService categoryService, IProductService productService, INavigationService navigationService)
        {
            _menuService = menuService;
            _categoryService = categoryService;
            _productService = productService;
            _navigationService = navigationService;
            AddProductToMenuCommand = new RelayCommand(AddProductToMenu);
            RemoveProductFromMenuCommand = new RelayCommand(RemoveProductFromMenu);
            SaveCommand = new RelayCommand(SaveMenu);
            CancelCommand = new RelayCommand(() => _navigationService.NavigateToMenuList());
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            Categories = new ObservableCollection<Category>(await _categoryService.GetAllCategoriesAsync());
            Products = new ObservableCollection<Product>(await _productService.GetAllProductsAsync());
        }

        private void AddProductToMenu()
        {
            if (SelectedProduct != null && !SelectedProducts.Contains(SelectedProduct))
                SelectedProducts.Add(SelectedProduct);
        }

        private void RemoveProductFromMenu()
        {
            if (SelectedProductInMenu != null)
                SelectedProducts.Remove(SelectedProductInMenu);
        }

        private void SaveMenu()
        {
            // Implementare salvare meniu
            _navigationService.NavigateToMenuList();
        }
    }
} 