using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class MenuDish
    {
        public int Id { get; set; }

        // Foreign keys
        [Required]
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; } = null!;

        [Required]
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; } = null!;

        // Quantity in grams for this dish when part of this menu
        public double PortionSize { get; set; }
    }
} 