using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Foreign keys
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        // Can be either a Dish or a Menu, but not both
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; } = null!;

        public int? MenuId { get; set; }
        public virtual Menu Menu { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
} 