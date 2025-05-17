using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views
{
    public partial class CategoryEditView : Page
    {
        public CategoryEditView(CategoryEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 