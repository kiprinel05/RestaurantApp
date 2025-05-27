using System.Collections.Generic;
using System.Linq;
using Restaurant.Models;

namespace Restaurant.Services
{
    public interface ICartService
    {
        List<CartItem> GetCartItems();
        void AddToCart(Product product, int quantity = 1);
        void AddToCart(Menu menu, int quantity = 1);
        void RemoveFromCart(int cartItemId);
        void UpdateQuantity(int cartItemId, int quantity);
        void ClearCart();
        decimal GetTotal();
    }

    public class CartService : ICartService
    {
        private readonly List<CartItem> _cartItems;
        private int _nextId = 1;

        public CartService()
        {
            _cartItems = new List<CartItem>();
        }

        public List<CartItem> GetCartItems() => _cartItems;

        public void AddToCart(Product product, int quantity = 1)
        {
            var existingItem = _cartItems.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    Id = _nextId++,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity,
                    Price = product.Price
                });
            }
        }

        public void AddToCart(Menu menu, int quantity = 1)
        {
            var existingItem = _cartItems.FirstOrDefault(i => i.MenuId == menu.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    Id = _nextId++,
                    MenuId = menu.Id,
                    Menu = menu,
                    Quantity = quantity,
                    Price = menu.Price
                });
            }
        }

        public void RemoveFromCart(int cartItemId)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        public void UpdateQuantity(int cartItemId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }

        public decimal GetTotal()
        {
            return _cartItems.Sum(item => item.TotalPrice);
        }
    }
} 