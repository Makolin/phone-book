using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    // В зависимости от длины наименования подразделения выводит разные значения в DataGrid
    public class DisplayDepartment : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            var nameDepartment = string.Empty;
            if (value != null)
            {
                var currentDepartment = (Department)value;
                using (ApplicationContext db = new ApplicationContext())
                {
                    nameDepartment = $"{ currentDepartment.DepartmentFullName} ({currentDepartment.DepartmentNumber})";
                    if (nameDepartment.Length < 40) return nameDepartment;
                    else
                    {
                        nameDepartment = $"{ currentDepartment.DepartmentShortName} ({currentDepartment.DepartmentNumber})";
                        return nameDepartment;
                    }
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}