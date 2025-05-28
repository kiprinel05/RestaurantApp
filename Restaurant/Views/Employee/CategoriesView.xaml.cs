using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views.Employee
{
    public partial class CategoriesView : UserControl
    {
        public CategoriesView(CategoriesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 