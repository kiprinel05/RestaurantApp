using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class Dish
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Portion size in grams
        public double PortionSize { get; set; }

        // Total quantity available in restaurant in grams
        public double TotalQuantityAvailable { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Foreign key for Category
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        // Navigation properties
        public virtual ICollection<Allergen> Allergens { get; set; }
        public virtual ICollection<DishImage> Images { get; set; }
        public virtual ICollection<MenuDish> MenuDishes { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Dish()
        {
            Allergens = new HashSet<Allergen>();
            Images = new HashSet<DishImage>();
            MenuDishes = new HashSet<MenuDish>();
            OrderItems = new HashSet<OrderItem>();
        }

        [NotMapped]
        public bool IsAvailable => TotalQuantityAvailable >= PortionSize;
    }
} 