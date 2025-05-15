using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }

        public Category()
        {
            Dishes = new HashSet<Dish>();
            Menus = new HashSet<Menu>();
        }
    }
} 