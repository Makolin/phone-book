﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    class DisplayCountDepartment : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            if (value != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    int idDepartment = (int)value;
                    var find = db.Deparments.Where(t => t.DepartmentId == idDepartment);
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
        public object ConvertBack(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}