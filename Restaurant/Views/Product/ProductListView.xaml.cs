using System.Windows.Controls;

namespace Restaurant.Views
{
    public partial class ProductListView : Page
    {
        public ProductListView(ViewModels.ProductListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 