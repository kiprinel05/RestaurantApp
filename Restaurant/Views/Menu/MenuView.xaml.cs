using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views
{
    public partial class MenuView : Page
    {
        public MenuView(MenuListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 