using System;
using System.Windows.Controls;

namespace Restaurant.Services
{
    public interface INavigationService
    {
        void NavigateTo(Type viewType);
        void NavigateTo(Type viewType, object parameter);
        void NavigateToMain();
        void NavigateToAuth();
        void NavigateToMenu();
        void NavigateToEmployeeDashboard();
        void NavigateToCategoryList();
        void NavigateToCategoryEdit(int? categoryId);
        void NavigateToProductList();
        void NavigateToProductEdit(int? productId);
        void NavigateToProductAdd();
        void NavigateToMenuList();
        void NavigateToMenuAdd();
        void NavigateBack();
        Page? CurrentPage { get; }
    }
} 