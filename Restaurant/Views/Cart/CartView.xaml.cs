using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.Cart
{
    public partial class CartView : UserControl
    {
        public CartView(CartViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 