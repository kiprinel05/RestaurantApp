using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Foreign key pentru categorie
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // Relatia many-to-many cu produsele si cantitatile lor specifice pentru acest meniu
        public virtual ICollection<MenuProduct> MenuProducts { get; set; } = new List<MenuProduct>();
    }
} 