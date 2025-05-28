using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Restaurant.Models;

namespace Restaurant.Views.Menu
{
    public class OrderCancelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus status)
            {
                return (status == OrderStatus.Registered || status == OrderStatus.InPreparation)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 