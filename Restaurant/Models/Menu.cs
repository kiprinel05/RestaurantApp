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
                return BasePrice;
            }
        }

        public bool IsAvailable 
        { 
            get
            {
                return MenuProducts.All(mp => mp.Product.IsAvailable);
            }
        }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<MenuProduct> MenuProducts { get; set; } = new List<MenuProduct>();

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