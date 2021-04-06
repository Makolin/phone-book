using Phone_Book.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Phone_Book.Pages
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommonNumber.Content = "Общий многоканальный номер 344 - 154";
            DataContext = new UserCollection();
        }

        // Закрытие текущего окна приложения
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Вывод окна с отображением основной информации о приложении
        private void MenuInfo_Click(object sender, RoutedEventArgs e)
        {
            AboutPage aboutPage = new AboutPage();
            aboutPage.ShowDialog();
        }

        // Создание нового пользователя
        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            EditUserPage newUserPage = new EditUserPage(null);
            newUserPage.ShowDialog();
        }

        // Редактирование выбранного из списка пользователя
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var editUser = (User)UsersGrid.SelectedItem;
            if (editUser != null)
            {
                if (!editUser.Common)
                {
                    EditUserPage editUserPage = new EditUserPage(editUser);
                    editUserPage.ShowDialog();
                }
                else
                {
                    EditCommonUserPage editCommonPage = new EditCommonUserPage(editUser);
                    editCommonPage.ShowDialog();
                }
            }
        }

        // Удаление выбранного из списка пользователя
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var deleteUser = (User)UsersGrid.SelectedItem;
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
            var findString = FindString.Text;
            DataContext = new UserCollection(findString);
        }

        // Поиск пользователя после нажатия определенных клавиш в строке ввода 
        private void FindString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var findString = FindString.Text;
                DataContext = new UserCollection(findString);
            }
        }

        // Создание нового абонента (общего номера телефона)
        private void NewCommon_Click(object sender, RoutedEventArgs e)
        {
            EditCommonUserPage newCommonPage = new EditCommonUserPage(null);
            newCommonPage.ShowDialog();
        }

        // Обновление статуса онлайна пользователя
        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckOnline.SetStatus();
        }

        // Редактирование подразделений в базе данных
        private void EditDepartment_Click(object sender, RoutedEventArgs e)
        {
            ListDepartment EditDepartment = new ListDepartment();
            EditDepartment.ShowDialog();
        }

        // Отображение основной информации о пользователе
        private void UsersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var editUser = (User)UsersGrid.SelectedItem;
            if (editUser != null)
            {
                /*ViewUser editAbsence = new ViewUser(editUser);
                editAbsence.ShowDialog();*/
            }
        }
    }
}
