using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Models;
using System.Collections.ObjectModel;
using Restaurant.Services;

namespace Restaurant.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ProductViewModel> products = new();

        [ObservableProperty]
        private ObservableCollection<MenuViewModel> menus = new();

        private readonly IServiceProvider _serviceProvider;

        public CategoryViewModel()
        {
        }

        public CategoryViewModel(Category category, IMenuService menuService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            
            Products = new ObservableCollection<ProductViewModel>(
                category.Products.Select(p => new ProductViewModel(p)));
            
            Menus = new ObservableCollection<MenuViewModel>(
                category.Menus.Select(m => new MenuViewModel(menuService, serviceProvider)));
        }
    }
} 