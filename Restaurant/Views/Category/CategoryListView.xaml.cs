using System.Windows.Controls;
using Restaurant.ViewModels;
using System.Windows;

namespace Restaurant.Views
{
    public partial class CategoryListView : Page
    {
        private CategoryListView()
        {
            InitializeComponent();
        }

        public CategoryListView(CategoryListViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
} 