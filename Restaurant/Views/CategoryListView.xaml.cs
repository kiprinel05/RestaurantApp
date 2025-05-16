using System.Windows.Controls;
using Restaurant.ViewModels;

namespace Restaurant.Views
{
    public partial class CategoryListView : Page
    {
        public CategoryListView()
        {
            InitializeComponent();
        }

        public CategoryListView(CategoryListViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
} 