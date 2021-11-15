using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    // Разбивает городской номер телефона на три блока с помощью дефисов
    public class DisplayCityNumber : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string cityNumber = value.ToString();
                cityNumber = cityNumber.Insert(2, "-");
                cityNumber = cityNumber.Insert(5, "-");
                return cityNumber;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
