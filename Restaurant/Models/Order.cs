using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Restaurant.Models
{
    public enum OrderStatus
    {
        Registered,
        InPreparation,
        OutForDelivery,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderCode { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        // Foreign key for User
        public int UserId { get; set; }
        public virtual User User { get; set; }

        // Navigation property
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderDate = DateTime.Now;
            OrderItems = new HashSet<OrderItem>();
            Status = OrderStatus.Registered;
            OrderCode = GenerateOrderCode();
        }

        [NotMapped]
        public decimal SubTotal => OrderItems?.Sum(item => item.TotalPrice) ?? 0;

        [NotMapped]
        public decimal Total => SubTotal + DeliveryFee - Discount;

        private string GenerateOrderCode()
        {
            return $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
} 