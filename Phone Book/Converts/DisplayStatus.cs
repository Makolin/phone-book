using System;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    public class DisplayStatus : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            if (value != null)
            {
                if ((bool)value) return "Да";
                else return "Нет";
            }

            // Иначе просто вернуть пустое значение
            return string.Empty;
        }
        public object ConvertBack(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
