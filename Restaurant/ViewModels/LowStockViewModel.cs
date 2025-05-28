using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Restaurant.Models;
using Restaurant.Services.Interfaces;
using Restaurant.Services;

namespace Restaurant.ViewModels
{
    public partial class LowStockViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        [ObservableProperty]
        private ObservableCollection<LowStockItem> lowStockItems = new();

        public LowStockViewModel(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;
            LoadLowStockItemsAsync();
        }

        private async void LoadLowStockItemsAsync()
        {
            var threshold = _configuration.GetValue<int>("StockSettings:LowStockThreshold");
            var products = await _productService.GetAllProductsAsync();
            var lowStock = products.Where(p => p.TotalQuantity / p.PortionQuantity <= threshold)
                                  .Select(p => new LowStockItem { Name = p.Name, Quantity = p.TotalQuantity / p.PortionQuantity });
            LowStockItems = new ObservableCollection<LowStockItem>(lowStock);
            System.Diagnostics.Debug.WriteLine($"Loaded {LowStockItems.Count} low stock items. Threshold: {threshold}");
        }
    }

    public class LowStockItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
} 