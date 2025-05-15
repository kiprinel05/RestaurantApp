using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class DishImage
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImagePath { get; set; }

        // Foreign key for Dish
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }
    }
} 