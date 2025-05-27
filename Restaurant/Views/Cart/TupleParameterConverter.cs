using System;
using System.Globalization;
using System.Windows.Data;

namespace Restaurant.Views.Cart
{
    public class TupleParameterConverter : IValueConverter
    {
        public static readonly TupleParameterConverter Instance = new TupleParameterConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = value is int i ? i : 0;
            int delta = 0;
            if (parameter is int d)
                delta = d;
            else if (parameter is string s && int.TryParse(s, out int parsed))
                delta = parsed;
            return (id, delta);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 