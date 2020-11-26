using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Phone_Book
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public ViewModel(string findString)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (findString == string.Empty)
                {
                    users = new ObservableCollection<User>(db.Users
                        .Include(t => t.Department)
                        .Include(t => t.LocalNumber)
                        .Include(t => t.CityNumber)
                        .ToList());
                }
                else
                {
                    findString = findString.ToLower().Trim();
                    users = new ObservableCollection<User>(db.Users
                        .Include(t => t.Department)
                        .Include(t => t.LocalNumber)
                        .Include(t => t.CityNumber)
                        .Where(t => EF.Functions.Like(t.Name, $"%{findString}%"))
                        .ToList());
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
