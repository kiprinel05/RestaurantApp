using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Restaurant.Models;

namespace Restaurant.Views.Menu
{
    public class StatusToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0 && values[0] is OrderStatus status)
            {
                switch (status)
                {
                    case OrderStatus.Registered:
                        return Brushes.DodgerBlue;
                    case OrderStatus.InPreparation:
                        return Brushes.Orange;
                    case OrderStatus.InDelivery:
                        return Brushes.MediumPurple;
                    case OrderStatus.Delivered:
                        return Brushes.Green;
                    case OrderStatus.Cancelled:
                        return Brushes.Red;
                }
            }
            return Brushes.Gray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 