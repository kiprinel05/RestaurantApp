using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int PortionQuantity { get; set; }  // cantitatea per portie in grame

        [Required]
        public int TotalQuantity { get; set; }    // cantitatea totala disponibila in restaurant

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string ImagePath { get; set; } = string.Empty;

        // Foreign key pentru categorie
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // many-to-many cu alergenii
        public virtual ICollection<Allergen> Allergens { get; set; } = new List<Allergen>();

        // lista de imagini
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public bool IsAvailable { get; set; }
        public int PrepTime { get; set; } // Time in minutes
    }
} 