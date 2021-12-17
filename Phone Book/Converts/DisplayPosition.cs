using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    // Вывод должности абонента
    internal class DisplayPosition : IValueConverter
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
                        var position = db.Positions.Where(t => t.PositionId == user.PositionId).FirstOrDefault();
                        if (position != null)
                        {
                            return position.PositionName;
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
