using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
        public virtual ICollection<Menu> Menus { get; set; }

        public Category()
        {
            Menus = new HashSet<Menu>();
        }
    }
} 