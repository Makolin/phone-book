using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace Phone_Book.Model
{
    // Создание коллекции из базы данных, которая отображается на главной странице приложения.
    public class UserCollection
    {
        public static ObservableCollection<User> Users { get; set; }
        public static string CountUserString { get; set; }
        public UserCollection()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Users = new ObservableCollection<User>(db.Users
                    .Include(t => t.Position)
                    .Include(t => t.Department)
                    .Include(t => t.LocalNumber)
                    .Include(t => t.CityNumber)
                    .Include(t => t.ComputerStatus)
                    .OrderBy(t => t.Name)
                    .ToList());
            }
            CountUserString = $"В базе данных найдено {Users.Count} записей";
        }
        public UserCollection(string findString)
        {
            findString = findString.ToLower().Trim();
            using (ApplicationContext db = new ApplicationContext())
            {
                Users = new ObservableCollection<User>(db.Users
                    .Include(t => t.Position)
                    .Include(t => t.Department)
                    .Include(t => t.LocalNumber)
                    .Include(t => t.CityNumber)
                    .Include(t => t.ComputerStatus)
                    .Where(t => EF.Functions.Like(t.Name.ToLower(), $"%{findString}%")
                    || EF.Functions.Like(t.LocalNumber.LocalNumber.ToString(), $"%{findString}%"))
                    .OrderBy(t => t.Name)
                    .ToList());
            }
            CountUserString = $"В базе данных найдено {Users.Count} записей";
        }
    }
}
