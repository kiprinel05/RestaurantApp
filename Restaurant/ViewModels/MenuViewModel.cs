using CommunityToolkit.Mvvm.ComponentModel;
using Restaurant.Models;
using System.Collections.ObjectModel;

namespace Restaurant.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        public class MenuProductItem
        {
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public string QuantityDisplay => $"{Quantity}g";
        }

        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private string category = string.Empty;

        [ObservableProperty]
        private bool isAvailable;

        [ObservableProperty]
        private ObservableCollection<MenuProductItem> menuItems = new();

        public MenuViewModel()
        {
        }

        public MenuViewModel(Menu menu)
        {
            Id = menu.Id;
            Name = menu.Name;
            Description = menu.Description;
            Price = menu.Price;
            Category = menu.Category.Name;
            
            MenuItems = new ObservableCollection<MenuProductItem>(
                menu.MenuProducts.Select(mp => new MenuProductItem 
                { 
                    ProductName = mp.Product.Name,
                    Quantity = mp.Quantity
                }));

            IsAvailable = menu.MenuProducts.All(mp => mp.Product.TotalQuantity > 0);
        }
    }
} 