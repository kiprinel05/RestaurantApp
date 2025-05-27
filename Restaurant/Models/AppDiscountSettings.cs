namespace Restaurant.Models
{
    public class AppDiscountSettings
    {
        public DiscountSettings DiscountSettings { get; set; }
        public DeliverySettings DeliverySettings { get; set; }
    }

    public class DiscountSettings
    {
        public double DiscountThreshold { get; set; }
        public int DiscountOrderCount { get; set; }
        public int DiscountOrderIntervalDays { get; set; }
        public double DiscountPercent { get; set; }
    }

    public class DeliverySettings
    {
        public double DeliveryThreshold { get; set; }
        public double DeliveryFee { get; set; }
    }
} 