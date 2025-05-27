using System.Windows;
using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.Menu
{
    public partial class MenuView : Page
    {
        private MenuViewModel _viewModel;

        public MenuView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (MenuViewModel)this.DataContext;
            if (_viewModel != null)
            {
                MenuButton_Click(null, null); // Set initial view
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null) return;
            UpdateButtonStates(MenuButton);
            _viewModel.NavigateToMenu();
        }

        private void MyOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null) return;
            UpdateButtonStates(MyOrdersButton);
            _viewModel.NavigateToOrders();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null) return;
            UpdateButtonStates(CartButton);
            _viewModel.NavigateToCart();
        }

        private void UpdateButtonStates(Button activeButton)
        {
            if (MenuButton == null || MyOrdersButton == null || CartButton == null) return;

            MenuButton.Background = null;
            MyOrdersButton.Background = null;
            CartButton.Background = null;

            activeButton.Background = (System.Windows.Media.Brush)this.Resources["AccentBrush"];
        }
    }
} 