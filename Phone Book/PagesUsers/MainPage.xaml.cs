using Phone_Book.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Phone_Book.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            if (Authorization.authorization)
            {
                MainContextMenu.Visibility = Visibility.Visible;
            }

            CommonNumber.Content = "Общий многоканальный номер 499 - 080";
            DataContext = new UserCollection();
            //MainWindow.CountUser.Content = CommonNumber.Content;
        }

        // Создание нового пользователя
        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            EditUserWindow newUserPage = new EditUserWindow(null);
            newUserPage.ShowDialog();
        }

        // Редактирование выбранного из списка пользователя
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            User editUser = (User)UsersGrid.SelectedItem;
            if (editUser != null)
            {
                if (!editUser.Common)
                {
                    EditUserWindow editUserPage = new EditUserWindow(editUser);
                    editUserPage.ShowDialog();
                }
                else
                {
                    EditCommonUserWindow editCommonPage = new EditCommonUserWindow(editUser);
                    editCommonPage.ShowDialog();
                }
            }
        }

        // Удаление выбранного из списка пользователя
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            User deleteUser = (User)UsersGrid.SelectedItem;
            if (deleteUser != null)
            {
                MessageBoxResult result = MessageBox.Show(
                     $"Вы действительно хотите удалить пользователя с именем \"{deleteUser.Name}\" из базы данных?",
                     "Удалить",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        UserCollection.Users.Remove(deleteUser);
                        db.Users.Remove(deleteUser);
                        db.SaveChanges();
                    }
                }
            }
        }

        // Поиск пользователя после нажатия кнопки "Найти"
        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            string findString = FindString.Text;
            DataContext = new UserCollection(findString);
        }

        // Поиск пользователя после нажатия определенных клавиш в строке ввода 
        private void FindString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string findString = FindString.Text;
                DataContext = new UserCollection(findString);
            }
        }

        // Создание нового абонента (общего номера телефона)
        private void NewCommon_Click(object sender, RoutedEventArgs e)
        {
            EditCommonUserWindow newCommonPage = new EditCommonUserWindow(null);
            newCommonPage.ShowDialog();
        }

        // Обновление статуса онлайна пользователя
        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckOnline.SetStatus();
        }

        // Отображение основной информации о пользователе
        private void UsersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            User editUser = (User)UsersGrid.SelectedItem;
            if (editUser != null)
            {
                /*ViewUser editAbsence = new ViewUser(editUser);
                editAbsence.ShowDialog();*/
            }
        }
    }
}
