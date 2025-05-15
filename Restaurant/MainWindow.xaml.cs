using Restaurant.ViewModels;
using System.Windows;

namespace Restaurant
{
    public partial class MainWindow : Window
    {
        private readonly required MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
} 