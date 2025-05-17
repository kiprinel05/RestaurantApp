using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OrderCode { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Registered;

        public DateTime? EstimatedDeliveryTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public required User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
} 