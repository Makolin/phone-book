using System;
using System.Linq;

namespace Phone_Book
{
    // Приветствие пользователя и дает разрешение редактировать данные в справочнике
    internal static class Authorization
    {
        
        public static bool authorization = false;

        public static string HelloUser()
        {
            string nameUser = Environment.UserName;
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.Where(t => t.DomainName == nameUser).FirstOrDefault();
                if (user != null)
                {
                    nameUser = "Текущий пользователь: " + user.Name;
                    authorization = true;
                    return nameUser;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
