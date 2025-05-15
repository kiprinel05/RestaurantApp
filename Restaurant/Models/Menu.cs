using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Restaurant.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public virtual ICollection<MenuDish> MenuDishes { get; set; } = new List<MenuDish>();

        // Foreign key for Category
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        // Navigation property
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Menu()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [NotMapped]
        public decimal CalculatedPrice
        {
            get
            {
                if (MenuDishes == null || !MenuDishes.Any())
                    return 0;

                decimal totalPrice = MenuDishes.Sum(md => md.Dish.Price);
                // The discount percentage will be applied in the business logic layer
                // as it comes from configuration
                return totalPrice;
            }
        }

        [NotMapped]
        public bool IsAvailable => MenuDishes?.All(md => md.Dish.IsAvailable) ?? false;
    }
} 