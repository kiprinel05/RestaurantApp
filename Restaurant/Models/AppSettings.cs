namespace Restaurant.Models
{
    public class AppSettings
    {
        // Menu discount percentage
        public decimal MenuDiscountPercentage { get; set; }

        // Order settings
        public decimal MinimumOrderValueForFreeDelivery { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal OrderDiscountPercentage { get; set; }
        public int MinimumOrdersForDiscount { get; set; }
        public int DiscountTimeWindowInDays { get; set; }

        // Inventory settings
        public double LowStockThreshold { get; set; }
    }
} 