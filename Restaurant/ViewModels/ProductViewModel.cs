using CommunityToolkit.Mvvm.ComponentModel;
using Restaurant.Models;
using System.Collections.ObjectModel;

namespace Restaurant.ViewModels
{
    public partial class ProductViewModel : ObservableObject
    {
        [ObservableProperty]
        private int id;

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
        private string category = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> allergens = new();

        [ObservableProperty]
        private ObservableCollection<string> imagePaths = new();

        public ProductViewModel()
        {
        }

        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            PortionQuantity = product.PortionQuantity;
            TotalQuantity = product.TotalQuantity;
            IsAvailable = TotalQuantity > 0;
            Category = product.Category.Name;
            
            Allergens = new ObservableCollection<string>(
                product.Allergens.Select(a => a.Name));
            
            ImagePaths = new ObservableCollection<string>(
                product.Images.Select(i => i.ImagePath));
        }
    }
} 