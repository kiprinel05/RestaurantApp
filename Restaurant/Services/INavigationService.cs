using System;

namespace Restaurant.Services
{
    public interface INavigationService
    {
        void NavigateTo(Type viewType);
        void NavigateToMain();
    }
} 