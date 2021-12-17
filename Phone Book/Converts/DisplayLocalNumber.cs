using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    // Вывод местного номера телефона абонента
    internal class DisplayLocalNumber : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                using (ApplicationContext db = new())
                {
                    var user = db.Users.Where(t => t.UserId == (int)value).FirstOrDefault();
                    if (user != null)
                    {
                        var localNumber = db.Locals.Where(t => t.LocalId == user.LocalId).FirstOrDefault();
                        if (localNumber != null)
                        {
                            return localNumber.LocalNumber;
                        }
                    }
                }
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
