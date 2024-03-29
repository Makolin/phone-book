using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    // Если у пользователя есть отсутствие или скоро будет, то выводит его в основной DataGrid
    public class DisplayAbsence : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                int userId = (int)value;
                List<Absence> AbsenceList = new List<Absence>(db.Absences
                    .Where(t => t.UserId == userId))
                    .Where(t => t.DateBefore.Date >= DateTime.Today)
                    .Where(t => t.DateFrom.Date <= DateTime.Today.AddDays(5))
                    .ToList();

                if (AbsenceList.Count > 0)
                {
                    string absenceString = string.Empty;
                    foreach (Absence absence in AbsenceList)
                    {
                        absenceString += $"{absence.Reason} c {absence.DateFrom:dd.MM} до {absence.DateBefore:dd.MM}";
                    }
                    return absenceString;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
