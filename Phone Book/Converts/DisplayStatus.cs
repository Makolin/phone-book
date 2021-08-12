using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Phone_Book.Converts
{
    // Превращает строковый статус текущего онлайна пользователя в графический, подставляя при этом картинку определенного цвета.
    public class DisplayStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmap;
            if (value != null)
            {
                if ((bool)value)
                {
                    bitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/GreenCircle.png"));
                    return bitmap;
                }
                else
                {
                    bitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/RedCircle.png"));
                    return bitmap;
                }
            }
            bitmap = new BitmapImage(new Uri("pack://application:,,,/Resources/GrayCircle.png"));
            return bitmap;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
