using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using Restaurant.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.ViewModels
{
    public class CartViewModel : ViewModelBase
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authService;
        private readonly AppSettingsProvider _settingsProvider;
        private ObservableCollection<CartItem> _cartItems;
        private decimal _total;
        private decimal _discount;
        private decimal _deliveryFee;
        private decimal _subTotal;
        private decimal _totalFinal;
        private bool _isPlacingOrder;

        public CartViewModel(ICartService cartService, IOrderService orderService, IAuthenticationService authService, AppSettingsProvider settingsProvider)
        {
            _cartService = cartService;
            _orderService = orderService;
            _authService = authService;
            _settingsProvider = settingsProvider;
            _cartItems = new ObservableCollection<CartItem>(_cartService.GetCartItems());
            _total = _cartService.GetTotal();
            PlaceOrderCommand = new AsyncRelayCommand(PlaceOrderAsync, CanPlaceOrder);
            _ = RefreshCartAsync();
        }

        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set
            {
                _cartItems = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanPlaceOrder));
            }
        }

        public decimal SubTotal
        {
            get => _subTotal;
            set { _subTotal = value; OnPropertyChanged(); }
        }
        public decimal Discount
        {
            get => _discount;
            set { _discount = value; OnPropertyChanged(); }
        }
        public decimal DeliveryFee
        {
            get => _deliveryFee;
            set { _deliveryFee = value; OnPropertyChanged(); }
        }
        public decimal TotalFinal
        {
            get => _totalFinal;
            set { _totalFinal = value; OnPropertyChanged(); }
        }
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlacingOrder
        {
            get => _isPlacingOrder;
            set { _isPlacingOrder = value; OnPropertyChanged(); }
        }

        public ICommand PlaceOrderCommand { get; }
        public bool CanPlaceOrder() => CartItems.Count > 0 && !IsPlacingOrder;

        public ICommand UpdateQuantityCommand => new RelayCommand<(int cartItemId, int delta)>(
            param =>
            {
                var (cartItemId, delta) = param;
                var item = _cartService.GetCartItems().FirstOrDefault(i => i.Id == cartItemId);
                if (item != null)
                {
                    int newQuantity = item.Quantity + delta;
                    if (newQuantity > 0)
                    {
                        _cartService.UpdateQuantity(cartItemId, newQuantity);
                        _ = RefreshCartAsync();
                    }
                }
            });

        public ICommand RemoveItemCommand => new RelayCommand<int>(
            cartItemId =>
            {
                _cartService.RemoveFromCart(cartItemId);
                _ = RefreshCartAsync();
            });

        private async Task RefreshCartAsync()
        {
            CartItems = new ObservableCollection<CartItem>(_cartService.GetCartItems());
            Total = _cartService.GetTotal();
            SubTotal = Total;
            Discount = 0;
            DeliveryFee = 0;
            var settings = _settingsProvider.GetSettings();
            var user = _authService.GetCurrentUser();
            // Discount by value
            if (SubTotal >= (decimal)settings.DiscountSettings.DiscountThreshold)
            {
                Discount = SubTotal * ((decimal)settings.DiscountSettings.DiscountPercent / 100m);
            }
            else
            {
                // Discount by order count in interval
                var orders = await _orderService.GetOrdersForUserAsync(user.Id);
                var interval = TimeSpan.FromDays(settings.DiscountSettings.DiscountOrderIntervalDays);
                var recentOrders = orders.Where(o => o.OrderDate >= DateTime.Now - interval).ToList();
                if (recentOrders.Count >= settings.DiscountSettings.DiscountOrderCount)
                {
                    Discount = SubTotal * ((decimal)settings.DiscountSettings.DiscountPercent / 100m);
                }
            }
            // Delivery fee
            if (SubTotal - Discount < (decimal)settings.DeliverySettings.DeliveryThreshold)
            {
                DeliveryFee = (decimal)settings.DeliverySettings.DeliveryFee;
            }
            TotalFinal = SubTotal - Discount + DeliveryFee;
            (PlaceOrderCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
        }

        private async Task PlaceOrderAsync()
        {
            if (CartItems.Count == 0) return;
            IsPlacingOrder = true;
            var user = _authService.GetCurrentUser();
            var order = new Order
            {
                UserId = user.Id,
                User = user,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Registered,
                SubTotal = SubTotal,
                DeliveryFee = DeliveryFee,
                Discount = Discount,
                Total = TotalFinal,
                OrderItems = CartItems.Select(item => new OrderItem
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.TotalPrice,
                    ProductId = item.ProductId,
                    MenuId = item.MenuId
                }).ToList()
            };
            await _orderService.PlaceOrderAsync(order);
            _cartService.ClearCart();
            await RefreshCartAsync();
            IsPlacingOrder = false;
            // Optionally: show a message to the user
        }
    }
} 