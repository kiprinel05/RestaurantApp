using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services.Interfaces;
using Restaurant.Services;
using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Restaurant.Views.Controls;
using System.Windows;

namespace Restaurant.ViewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<OrderCardViewModel> orders = new();
        [ObservableProperty]
        private ObservableCollection<OrderCardViewModel> activeOrders = new();
        [ObservableProperty]
        private bool showActiveOrders;
        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ICommand ViewOrderDetailsCommand { get; }
        public ICommand DeleteOrderCommand { get; }
        public ICommand EditOrderStatusCommand { get; }

        public OrdersViewModel(IOrderService orderService, IAuthenticationService authService, INavigationService navigationService)
        {
            _orderService = orderService;
            _authService = authService;
            _navigationService = navigationService;
            ViewOrderDetailsCommand = new RelayCommand<int>(ViewOrderDetails);
            DeleteOrderCommand = new AsyncRelayCommand<int>(DeleteOrderAsync);
            EditOrderStatusCommand = new RelayCommand<int>(EditOrderStatus);
            LoadOrdersAsync();
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                var orders = await _orderService.GetAllOrdersForAdminAsync();
                if (ShowActiveOrders)
                {
                    Orders = new ObservableCollection<OrderCardViewModel>(
                        orders.Where(o => o.Status != OrderStatus.Cancelled && o.Status != OrderStatus.Delivered)
                              .Select(o => new OrderCardViewModel(o)));
                }
                else
                {
                    Orders = new ObservableCollection<OrderCardViewModel>(
                        orders.Select(o => new OrderCardViewModel(o)));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la încărcarea comenzilor: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ViewOrderDetails(int orderId)
        {
            // Navigare la detalii comandă (de implementat)
        }

        private async Task DeleteOrderAsync(int orderId)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                var user = _authService.GetCurrentUser();
                await _orderService.CancelOrderAsync(orderId, user.Id);
                await Task.Delay(200); // scurt delay pentru UX
                LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Eroare la ștergerea comenzii: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void EditOrderStatus(int orderId)
        {
            var dialog = new StatusSelectorDialog();
            var window = new Window
            {
                Content = dialog,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = null,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow,
                ShowInTaskbar = false
            };
            dialog.StatusSelected += async (newStatus) =>
            {
                window.Close();
                try
                {
                    IsLoading = true;
                    ErrorMessage = string.Empty;
                    await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
                    await Task.Delay(200);
                    LoadOrdersAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Eroare la actualizarea statusului: {ex.Message}";
                }
                finally
                {
                    IsLoading = false;
                }
            };
            window.ShowDialog();
        }
    }

    public class OrderCardViewModel : ObservableObject
    {
        public int Id { get; }
        public string OrderCode { get; }
        public string ClientName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string DeliveryAddress { get; }
        public DateTime OrderDate { get; }
        public OrderStatus Status { get; }
        public decimal SubTotal { get; }
        public decimal Discount { get; }
        public decimal DeliveryFee { get; }
        public decimal Total { get; }
        public ObservableCollection<OrderItem> OrderItems { get; }

        public OrderCardViewModel(Order order)
        {
            Id = order.Id;
            OrderCode = order.OrderCode;
            ClientName = order.User != null ? $"{order.User.FirstName} {order.User.LastName}" : "-";
            Email = order.User?.Email ?? "-";
            PhoneNumber = order.User?.PhoneNumber ?? "-";
            DeliveryAddress = order.User?.DeliveryAddress ?? "-";
            OrderDate = order.OrderDate;
            Status = order.Status;
            SubTotal = order.SubTotal;
            Discount = order.Discount;
            DeliveryFee = order.DeliveryFee;
            Total = order.Total;
            OrderItems = new ObservableCollection<OrderItem>(order.OrderItems);
        }
    }
} 