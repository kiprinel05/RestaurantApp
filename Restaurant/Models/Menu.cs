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
        public decimal BasePrice { get; set; }  // pretul calculat din suma produselor

        [NotMapped]
        public decimal Price  // pretul final cu reducere
        {
            get
            {
                // logica de calcul in service
                return BasePrice;
            }
        }

        public bool IsAvailable 
        { 
            get
            {
                // un meniu este disponibil daca toate produsele sale sunt disponibile
                return MenuProducts.All(mp => mp.Product.IsAvailable);
            }
        }

        // Foreign key pentru categorie
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // many-to-many cu produsele si cantitatile lor specifice pentru acest meniu
        public virtual ICollection<MenuProduct> MenuProducts { get; set; } = new List<MenuProduct>();

        // timpul total de preparare in minute
        public int TotalPrepTime
        {
            get
            {
                return MenuProducts.Sum(mp => mp.Product.PrepTime);
            }
        }

        [StringLength(500)]
        public string ImagePath { get; set; } = string.Empty;
    }
} 