using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Restaurant.Models;
using Restaurant.Services;
using System.Linq;

namespace Restaurant.ViewModels
{
    public class CartViewModel : ViewModelBase
    {
        private readonly ICartService _cartService;
        private ObservableCollection<CartItem> _cartItems;
        private decimal _total;

        public CartViewModel(ICartService cartService)
        {
            _cartService = cartService;
            _cartItems = new ObservableCollection<CartItem>(_cartService.GetCartItems());
            _total = _cartService.GetTotal();
        }

        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set
            {
                _cartItems = value;
                OnPropertyChanged();
            }
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
                        RefreshCart();
                    }
                }
            });

        public ICommand RemoveItemCommand => new RelayCommand<int>(
            cartItemId =>
            {
                _cartService.RemoveFromCart(cartItemId);
                RefreshCart();
            });

        private void RefreshCart()
        {
            CartItems = new ObservableCollection<CartItem>(_cartService.GetCartItems());
            Total = _cartService.GetTotal();
        }
    }
} 