using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Allergen
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<Dish> Dishes { get; set; }

        public Allergen()
        {
            Dishes = new HashSet<Dish>();
        }
    }
} 