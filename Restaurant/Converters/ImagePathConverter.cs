using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Restaurant.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return new BitmapImage(new Uri("pack://application:,,,/Images/default-menu.jpg"));

            string imagePath = value.ToString();
            
            // If it's already a full URI, return it as is
            if (Uri.IsWellFormedUriString(imagePath, UriKind.Absolute))
                return new BitmapImage(new Uri(imagePath));

            // If it's a relative path, make it absolute
            if (!imagePath.StartsWith("/"))
                imagePath = "/" + imagePath;

            // Add the pack URI prefix if not present
            if (!imagePath.StartsWith("pack://"))
                imagePath = "pack://application:,,," + imagePath;

            try
            {
                return new BitmapImage(new Uri(imagePath));
            }
            catch
            {
                // Return default image if loading fails
                return new BitmapImage(new Uri("pack://application:,,,/Images/default-menu.jpg"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 