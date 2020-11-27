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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new UserViewModel("");
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
            DataContext = new UserViewModel(findString);
        }

        // Создание нового пользователя
        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            EditUserPage newUser = new EditUserPage(null);
            newUser.Show();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (MainTable.SelectedItem != null)
            {
                User selectedUser = (User)MainTable.SelectedItem;
                EditUserPage editUser = new EditUserPage(selectedUser);
                editUser.Show();
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (MainTable.SelectedItem != null)
            {
                MessageBox.Show("Будет удаление пользователя!");
            }
        }
    }
}
