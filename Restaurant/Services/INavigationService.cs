using System;

namespace Restaurant.Services
{
    public interface INavigationService
    {
        void NavigateTo(Type viewType);
        void NavigateTo(Type viewType, object parameter);
        void NavigateToMain();
        void NavigateToAuth();
        void NavigateToMenu();
        void NavigateBack();
    }
} 