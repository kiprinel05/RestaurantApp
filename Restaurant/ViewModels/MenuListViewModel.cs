using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public partial class MenuListViewModel : ObservableObject
    {
        private readonly IMenuService _menuService;

        [ObservableProperty]
        private ObservableCollection<CategoryViewModel> categories = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        public MenuListViewModel(IMenuService menuService)
        {
            _menuService = menuService;
            LoadMenuCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadMenu()
        {
            if (IsLoading) return;
            
            try
            {
                IsLoading = true;

                var dbCategories = await _menuService.GetAllCategoriesWithDetailsAsync();
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
                var categories = await _menuService.SearchMenuAsync(searchTerm);
                Categories = new ObservableCollection<CategoryViewModel>(
                    categories.Select(c => new CategoryViewModel(c)));
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