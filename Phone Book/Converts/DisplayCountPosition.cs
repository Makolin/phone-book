using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    class DisplayCountPosition : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    int idPosition = (int)value;
                    var find = db.Users.Where(t => t.PositionId == idPosition);
                    if (find != null)
                    {
                        return find.Count().ToString();
                    }
                    else
                    {
                        return 0;
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
