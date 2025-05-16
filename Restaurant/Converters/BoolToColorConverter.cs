using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Restaurant.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isAvailable)
            {
                return new SolidColorBrush(Color.FromRgb(
                    isAvailable ? (byte)46 : (byte)231,  // R
                    isAvailable ? (byte)204 : (byte)76,  // G
                    isAvailable ? (byte)113 : (byte)60   // B
                ));
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 