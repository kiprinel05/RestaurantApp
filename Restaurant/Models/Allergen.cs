using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Allergen
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();

        public Allergen()
        {
            Dishes = new HashSet<Dish>();
        }
    }
} 