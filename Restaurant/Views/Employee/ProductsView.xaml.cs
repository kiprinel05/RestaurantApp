using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.Employee
{
    public partial class ProductsView : UserControl
    {
        public ProductsView(ProductListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 