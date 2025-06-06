using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class MenuProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int MenuSpecificPortionQuantity { get; set; }

        [NotMapped]
        public bool IsAvailable
        {
            get
            {
                return Product != null && Product.IsAvailable && 
                       Product.TotalQuantity >= (MenuSpecificPortionQuantity * Quantity);
            }
        }
    }
} 