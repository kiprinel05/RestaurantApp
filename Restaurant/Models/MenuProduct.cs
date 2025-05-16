using System.ComponentModel.DataAnnotations;

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
        public int Quantity { get; set; }  // cantitatea specifica pentru acest produs in acest meniu
    }
} 