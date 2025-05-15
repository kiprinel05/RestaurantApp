using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Foreign keys
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        // Can be either a Dish or a Menu, but not both
        public int? DishId { get; set; }
        public virtual Dish Dish { get; set; }

        public int? MenuId { get; set; }
        public virtual Menu Menu { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
} 