using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurant.ViewModels
{
    public partial class MenuListViewModel : ObservableObject
    {
        private readonly IMenuService _menuService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<Menu> menus = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        public ICommand AddMenuCommand { get; }
        public ICommand RemoveMenuCommand { get; }

        public MenuListViewModel(IMenuService menuService, INavigationService navigationService)
        {
            _menuService = menuService;
            _navigationService = navigationService;
            AddMenuCommand = new RelayCommand(() => _navigationService.NavigateToMenuAdd());
            RemoveMenuCommand = new RelayCommand<int>(async id => await RemoveMenuAsync(id));
            LoadMenusAsync();
        }

        private async Task LoadMenusAsync()
        {
            var menuList = await _menuService.GetAllMenusAsync();
            Menus = new ObservableCollection<Menu>(menuList);
        }

        private async Task RemoveMenuAsync(int id)
        {
            await _menuService.DeleteMenuAsync(id);
            await LoadMenusAsync();
        }
    }
} 