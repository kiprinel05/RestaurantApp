using CommunityToolkit.Mvvm.ComponentModel;
using Restaurant.Models;
using Restaurant.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        private readonly IMenuService _menuService;
        private List<Category> _allCategories = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string searchQuery = string.Empty;

        [ObservableProperty]
        private bool searchByAllergen;

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();

        public MenuViewModel(IMenuService menuService)
        {
            _menuService = menuService;
            LoadMenuAsync();
        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterMenu();
        }

        partial void OnSearchByAllergenChanged(bool value)
        {
            FilterMenu();
        }

        private async void LoadMenuAsync()
        {
            IsLoading = true;
            try
            {
                _allCategories = await _menuService.GetAllCategoriesWithDetailsAsync();
                FilterMenu();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterMenu()
        {
            if (_allCategories == null) return;

            var filteredCategories = _allCategories.ToList();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var searchTerm = SearchQuery.ToLower();
                if (SearchByAllergen)
                {
                    // Filter by allergen
                    filteredCategories = filteredCategories
                        .Select(c => new Category
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Products = c.Products.Where(p => 
                                p.Allergens.Any(a => a.Name.ToLower().Contains(searchTerm))).ToList(),
                            Menus = c.Menus.Where(m => 
                                m.MenuProducts.Any(mp => 
                                    mp.Product.Allergens.Any(a => a.Name.ToLower().Contains(searchTerm)))).ToList()
                        })
                        .Where(c => c.Products.Any() || c.Menus.Any())
                        .ToList();
                }
                else
                {
                    // Filter by name/description
                    filteredCategories = filteredCategories
                        .Select(c => new Category
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Products = c.Products.Where(p => 
                                p.Name.ToLower().Contains(searchTerm) || 
                                p.Description.ToLower().Contains(searchTerm)).ToList(),
                            Menus = c.Menus.Where(m => 
                                m.Name.ToLower().Contains(searchTerm) || 
                                m.Description.ToLower().Contains(searchTerm)).ToList()
                        })
                        .Where(c => c.Products.Any() || c.Menus.Any())
                        .ToList();
                }
            }

            Categories.Clear();
            foreach (var category in filteredCategories)
            {
                Categories.Add(category);
            }
        }
    }
} 