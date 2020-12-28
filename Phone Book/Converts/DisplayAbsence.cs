using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Phone_Book.Converts
{
    public class DisplayAbsence : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parametr, CultureInfo culture)
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
                    foreach (var absence in AbsenceList)
                    {
                        absenceString += $"{absence.Reason} c {absence.DateFrom.ToString("dd.MM")} до {absence.DateBefore.ToString("dd.MM")}";
                    }
                    return absenceString;
                }
                else return string.Empty;
            }
        }

        public object ConvertBack(object value, Type TargetType, object parametr, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
