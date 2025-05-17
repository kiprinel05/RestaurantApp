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
        public decimal BasePrice { get; set; }  // Prețul calculat din suma produselor

        [NotMapped]
        public decimal Price  // Prețul final cu reducere
        {
            get
            {
                // Vom implementa logica de calcul în service
                return BasePrice;
            }
        }

        public bool IsAvailable 
        { 
            get
            {
                // Un meniu este disponibil doar dacă toate produsele sale sunt disponibile
                return MenuProducts.All(mp => mp.Product.IsAvailable);
            }
        }

        // Foreign key pentru categorie
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // Relatia many-to-many cu produsele si cantitatile lor specifice pentru acest meniu
        public virtual ICollection<MenuProduct> MenuProducts { get; set; } = new List<MenuProduct>();

        // Timpul total de preparare în minute
        public int TotalPrepTime
        {
            get
            {
                return MenuProducts.Sum(mp => mp.Product.PrepTime);
            }
        }
    }
} 