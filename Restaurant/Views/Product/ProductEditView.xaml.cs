using System.Windows.Controls;

namespace Restaurant.Views
{
    public partial class ProductEditView : Page
    {
        public ProductEditView(ViewModels.ProductEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 