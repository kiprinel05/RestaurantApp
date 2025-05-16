using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Restaurant.ViewModels
{
    public partial class MenuListViewModel : ObservableObject
    {
        private readonly RestaurantDbContext _context;

        [ObservableProperty]
        private ObservableCollection<CategoryViewModel> categories = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        public MenuListViewModel(RestaurantDbContext context)
        {
            _context = context;
            LoadMenuCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadMenu()
        {
            if (IsLoading) return;
            
            try
            {
                IsLoading = true;

                var dbCategories = await _context.Categories
                    .Include(c => c.Products)
                        .ThenInclude(p => p.Allergens)
                    .Include(c => c.Products)
                        .ThenInclude(p => p.Images)
                    .Include(c => c.Menus)
                        .ThenInclude(m => m.MenuProducts)
                            .ThenInclude(mp => mp.Product)
                    .ToListAsync();

                Categories = new ObservableCollection<CategoryViewModel>(
                    dbCategories.Select(c => new CategoryViewModel(c)));
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadMenu();
                return;
            }

            if (IsLoading) return;

            try
            {
                IsLoading = true;

                var searchTerm = SearchText.ToLower();

                var matchingProducts = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Allergens)
                    .Include(p => p.Images)
                    .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                               p.Description.ToLower().Contains(searchTerm) ||
                               p.Allergens.Any(a => a.Name.ToLower().Contains(searchTerm)))
                    .ToListAsync();

                var matchingMenus = await _context.Menus
                    .Include(m => m.Category)
                    .Include(m => m.MenuProducts)
                        .ThenInclude(mp => mp.Product)
                    .Where(m => m.Name.ToLower().Contains(searchTerm) ||
                               m.Description.ToLower().Contains(searchTerm))
                    .ToListAsync();

                // Obținem toate categoriile unice care conțin fie produse, fie meniuri care se potrivesc
                var categoriesWithProducts = matchingProducts.GroupBy(p => p.Category);
                var categoriesWithMenus = matchingMenus.GroupBy(m => m.Category);
                
                var allCategories = categoriesWithProducts.Select(g => g.Key)
                    .Union(categoriesWithMenus.Select(g => g.Key))
                    .ToList();

                // Creăm ViewModels pentru categoriile găsite
                Categories = new ObservableCollection<CategoryViewModel>(
                    allCategories.Select(c => new CategoryViewModel(c)));
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            SearchCommand.Execute(null);
        }
    }
} 