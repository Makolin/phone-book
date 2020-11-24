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

namespace Phone_Book
{
    public partial class MainWindow : Window
    {
        // Создание коллекции для загрузки данных в DataGrid
        public static ObservableCollection<User> GetData(string findString)
        {
            ObservableCollection<User> taskUser = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                if (findString == string.Empty)
                {
                    taskUser = new ObservableCollection<User>(db.Users
                        .Include(t => t.Department)
                        .Include(t => t.LocalNumber)
                        .Include(t => t.CityNumber)
                        .ToList());
                }
                else
                {
                    findString = findString.ToLower().Trim();
                    taskUser = new ObservableCollection<User>(db.Users
                        .Include(t => t.Department)
                        .Include(t => t.LocalNumber)
                        .Include(t => t.CityNumber)
                        .Where(t => EF.Functions.Like(t.Name, $"%{findString}%"))
                        .ToList());
                }
            }
            return taskUser;
        }
        public MainWindow()
        {
            InitializeComponent();
            ObservableCollection<User> taskUser = GetData(string.Empty);
            MainTable.ItemsSource = taskUser;
        }

        // Открытие окна для редактирования выбранного из таблицы пользователя
        private void MainTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            User selectedUser = (User)MainTable.SelectedItem;
            EditUserPage editUser = new EditUserPage(selectedUser);
            editUser.Show();
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

        // Поиск по базе данных
        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            var findString = FindString.Text;
            ObservableCollection<User> taskUser = GetData(findString);
            MainTable.ItemsSource = taskUser;
        }

        // Создание нового пользователя
        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            EditUserPage newUser = new EditUserPage(null);
            newUser.Show();
        }
    }
}
