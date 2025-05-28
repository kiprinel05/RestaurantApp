using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.Employee
{
    public partial class ProductEditView : UserControl
    {
        public ProductEditView(ProductEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 