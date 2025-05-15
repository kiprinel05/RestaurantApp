using Restaurant.ViewModels;
using System.Windows;

namespace Restaurant
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<MainViewModel>();
        }
    }
} 