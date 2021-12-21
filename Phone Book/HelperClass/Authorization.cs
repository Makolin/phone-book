using System;
using System.Linq;

namespace Phone_Book
{
    internal static class Authorization
    {
        public static bool authorization = false;
        // Приветствие пользователя и разрешение редактировать другие строки
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
