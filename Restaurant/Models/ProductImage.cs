using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string ImagePath { get; set; } = string.Empty;

        // Foreign key pentru produs
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
} 