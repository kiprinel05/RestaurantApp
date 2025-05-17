using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class AppSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MenuDiscountPercentage { get; set; } // x% - reducerea pentru meniuri

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderDiscountThreshold { get; set; } // y lei - pragul pentru reducere la comenzi

        [Required]
        public int OrderCountForDiscount { get; set; } // z comenzi - numărul de comenzi pentru reducere

        [Required]
        public int OrderTimeWindowHours { get; set; } // t ore - perioada de timp pentru comenzi

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FreeDeliveryThreshold { get; set; } // a lei - pragul pentru livrare gratuită

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryCost { get; set; } // b lei - costul livrării

        [Required]
        public int LowStockThreshold { get; set; } // c - pragul pentru notificări stoc redus

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal OrderDiscountPercentage { get; set; } // w% - procentul de reducere pentru comenzi

        [Required]
        public int EstimatedDeliveryTimeMinutes { get; set; } // timpul estimat pentru livrare
    }
} 