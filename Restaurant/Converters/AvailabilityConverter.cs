using System;
using System.Globalization;
using System.Windows.Data;

namespace Restaurant.Converters
{
    public class AvailabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isAvailable)
            {
                return isAvailable ? "Disponibil" : "Indisponibil";
            }
            return "Necunoscut";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 