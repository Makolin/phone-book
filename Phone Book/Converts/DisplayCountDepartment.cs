using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    class DisplayCountDepartment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    int idDepartment = (int)value;
                    var find = db.Users.Where(t => t.DepartmentId == idDepartment);
                    return find != null ? find.Count().ToString() : 0;
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
