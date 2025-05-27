using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services.Interfaces;
using Restaurant.Services;
namespace Restaurant.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authService;
        private ObservableCollection<Order> _orders = new();
        private bool _isLoading;

        public OrdersViewModel(IOrderService orderService, IAuthenticationService authService)
        {
            _orderService = orderService;
            _authService = authService;
            RefreshCommand = new AsyncRelayCommand(LoadOrdersAsync);
            CancelOrderCommand = new AsyncRelayCommand<Order>(CancelOrderAsync);
            _ = LoadOrdersAsync();
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set { _orders = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand CancelOrderCommand { get; }

        private async Task LoadOrdersAsync()
        {
            IsLoading = true;
            var user = _authService.GetCurrentUser();
            var orders = await _orderService.GetOrdersForUserAsync(user.Id);
            Orders = new ObservableCollection<Order>(orders);
            IsLoading = false;
        }

        private async Task CancelOrderAsync(Order order)
        {
            if (order == null) return;
            var user = _authService.GetCurrentUser();
            var result = await _orderService.CancelOrderAsync(order.Id, user.Id);
            if (result)
            {
                order.Status = OrderStatus.Cancelled;
                OnPropertyChanged(nameof(Orders));
            }
        }
    }
} 