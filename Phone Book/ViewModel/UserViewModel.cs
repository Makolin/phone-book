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
        private string findString;
        private ObservableCollection<User> users;

        private RelayCommand addCommand;
        private RelayCommand editCommand;
        private RelayCommand deleteCommand;
        private RelayCommand seachCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??= new RelayCommand(obj =>
                {
                    EditUserPage newUserPage = new EditUserPage(null);
                    newUserPage.Show();
                });
            }
        }
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??= new RelayCommand(obj =>
                {
                    if (obj != null)
                    {
                        var editUser = obj as User;
                        EditUserPage editUserPage = new EditUserPage(editUser);
                        editUserPage.Show();
                    }
                });
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??= new RelayCommand(obj =>
                {
                    if (obj != null)
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            var deleteUser = obj as User;
                            Users.Remove(selectedUser);
                            db.Users.Remove(deleteUser);
                            db.SaveChanges();
                        }
                        OnPropertyChanged("DeleteUser");
                    }
                });
            }
        }
        public RelayCommand SearchCommand
        {
            get
            {
                return seachCommand ??= new RelayCommand(obj =>
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        if (findString != null && findString != string.Empty)
                        {
                            Users.Clear();
                            findString = findString.ToLower().Trim();
                            foreach (var user in new ObservableCollection<User>(db.Users
                                .Include(t => t.Department)
                                .Include(t => t.LocalNumber)
                                .Include(t => t.CityNumber)
                                .Where(t => EF.Functions.Like(t.Name, $"%{findString}%"))
                                .OrderBy(t => t.Name)
                                .ToList()))
                            {
                                Users.Add(user);
                            }
                        }
                        else
                        {
                            Users.Clear();
                            foreach (var user in new ObservableCollection<User>(db.Users
                                 .Include(t => t.Department)
                                 .Include(t => t.LocalNumber)
                                 .Include(t => t.CityNumber)
                                 .OrderBy(t => t.Name)
                                 .ToList()))
                            {
                                Users.Add(user);
                            }
                        }

                    }
                    OnPropertyChanged("SearchUser");
                });
            }
        }

        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; }
        }
        public string FindString
        {
            get { return findString; }
            set { findString = value; }
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

        public UserViewModel()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Users = new ObservableCollection<User>(db.Users
                    .Include(t => t.Department)
                    .Include(t => t.LocalNumber)
                    .Include(t => t.CityNumber)
                    .OrderBy(t => t.Name)
                    .ToList());
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
