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
    public class UserViewModel : INotifyPropertyChanged
    {
        private User selectedUser;
        private ObservableCollection<User> users;

        private RelayCommand addCommand;
        private RelayCommand deleteCommand;
        public RelayCommand AddCommad
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        User user = new User();
                        Users.Insert(0, user);
                        SelectedUser = user;
                    }));
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand(obj =>
                    {
                        User user = new User();
                        Users.Remove(user);
                        SelectedUser = user;
                    }));
            }
        }

        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public UserViewModel(string findString)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (findString == string.Empty)
                {
                    users = new ObservableCollection<User>(db.Users
                        .Include(t => t.Department)
                        .Include(t => t.LocalNumber)
                        .Include(t => t.CityNumber)
                        .OrderBy(t => t.Name)
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
                        .OrderBy(t => t.Name)
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
