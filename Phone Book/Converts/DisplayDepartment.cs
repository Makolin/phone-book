using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    // В зависимости от длины наименования подразделения выводит разные значения в DataGrid
    public class DisplayDepartment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Department currentDepartment = (Department)value;
                using (ApplicationContext db = new ApplicationContext())
                {
                    string nameDepartment = currentDepartment.DepartmentFullName;
                    if (currentDepartment.DepartmentNumber != null)
                    {
                        nameDepartment += $" ({currentDepartment.DepartmentNumber})";
                    }
                    if (nameDepartment.Length < 40)
                    {
                        return nameDepartment;
                    }
                    else
                    {
                        nameDepartment = currentDepartment.DepartmentShortName;
                        if (currentDepartment.DepartmentNumber != null)
                        {
                            nameDepartment += $" ({currentDepartment.DepartmentNumber})";
                        }
                        return nameDepartment;
                    }
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
