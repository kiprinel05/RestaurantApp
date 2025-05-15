namespace Restaurant.Models
{
    public class MenuDish
    {
        public int Id { get; set; }

        // Foreign keys
        public int MenuId { get; set; }
        public int DishId { get; set; }

        // Navigation properties
        public virtual Menu Menu { get; set; }
        public virtual Dish Dish { get; set; }

        // Quantity in grams for this dish when part of this menu
        public double PortionSize { get; set; }
    }
} 