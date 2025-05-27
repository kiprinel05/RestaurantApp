using Restaurant.Models;

namespace Restaurant.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? MenuId { get; set; }
        public Product? Product { get; set; }
        public Menu? Menu { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public string Name => Product != null ? Product.Name : Menu?.Name ?? string.Empty;
        public string ImagePath => Product != null ? Product.ImagePath : Menu?.ImagePath ?? string.Empty;
    }
} 