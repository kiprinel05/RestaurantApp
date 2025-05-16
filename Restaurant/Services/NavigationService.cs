using System;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame ?? throw new ArgumentNullException(nameof(mainFrame));
        }

        public void NavigateTo(Type viewType)
        {
            if (!typeof(Page).IsAssignableFrom(viewType))
            {
                throw new ArgumentException($"Type {viewType.Name} is not a Page");
            }

            var page = Activator.CreateInstance(viewType) as Page;
            _mainFrame.Navigate(page);
        }

        public void NavigateToMain()
        {
            // TODO: Replace with your main view type
            NavigateTo(typeof(Views.MainView));
        }
    }
} 