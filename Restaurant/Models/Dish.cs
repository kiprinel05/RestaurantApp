using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PortionSize { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<DishImage> Images { get; set; } = new List<DishImage>();
        public virtual ICollection<Allergen> Allergens { get; set; } = new List<Allergen>();
        public virtual ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [NotMapped]
        public bool IsAvailable => TotalQuantityAvailable >= PortionSize;
    }
} 