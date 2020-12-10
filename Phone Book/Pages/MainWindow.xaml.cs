using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Phone_Book
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<User> GetData(string findString)
        {
            ObservableCollection<User> taskUser = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                taskUser = new ObservableCollection<User>(db.Users
                     .Include(t => t.Position)
                     .Include(t => t.Department)
                     .Include(t => t.LocalNumber)
                     .Include(t => t.CityNumber)
                     .OrderBy(t => t.Name)
                     .ToList());
                if (findString != string.Empty)
                {
                    findString = findString.ToLower().Trim();
                    taskUser = taskUser = new ObservableCollection<User>(db.Users
                     .Include(t => t.Position)
                     .Include(t => t.Department)
                     .Include(t => t.LocalNumber)
                     .Include(t => t.CityNumber)
                     .Where(t => EF.Functions.Like(t.Name.ToLower(), $"%{findString}%"))
                     .OrderBy(t => t.Name)
                     .ToList());
                }
            }
            // Сделать условия по склонению
            CountUser.Content = $"В базе данных найдено {taskUser.Count} записей";
            return taskUser;
        }
        public MainWindow()
        {
            InitializeComponent();
            CommonNumber.Content = "Общий многоканальный номер 344 - 154";
            ObservableCollection<User> taskUser = GetData(string.Empty);
            MainTable.ItemsSource = taskUser;
        }

        // Закрытие текущего кона приложения
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Вывод окна с отображением основной информации о приложении
        private void MenuInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вывод информации о текущей версии приложения.");
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            EditUserPage newUserPage = new EditUserPage(null);
            newUserPage.Show();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var editUser = (User)MainTable.SelectedItem;
            EditUserPage editUserPage = new EditUserPage(editUser);
            editUserPage.Show();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
             "Вы действительно хотите удалить данного пользователя?",
             "Удалить",
             MessageBoxButton.YesNo,
             MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var deleteUser = (User)MainTable.SelectedItem;
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Users.Remove(deleteUser);
                    db.SaveChanges();

                }
            }
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            var findString = FindString.Text;
            ObservableCollection<User> taskUser = GetData(findString);
            MainTable.ItemsSource = taskUser;
        }
    }
}
