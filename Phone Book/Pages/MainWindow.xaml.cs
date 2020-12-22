using Microsoft.EntityFrameworkCore;
using Phone_Book.Pages;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Phone_Book
{
    public partial class MainWindow : Window
    {
        static ObservableCollection<User> Users = new ObservableCollection<User>();

        private static void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
        public void GetData(string findString)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Users = new ObservableCollection<User>(db.Users
                     .Include(t => t.Position)
                     .Include(t => t.Department)
                     .Include(t => t.LocalNumber)
                     .Include(t => t.CityNumber)
                     .OrderBy(t => t.Name)
                     .ToList());
                if (findString != string.Empty)
                {
                    findString = findString.ToLower().Trim();
                    Users = new ObservableCollection<User>(db.Users
                     .Include(t => t.Position)
                     .Include(t => t.Department)
                     .Include(t => t.LocalNumber)
                     .Include(t => t.CityNumber)
                     .Where(t => EF.Functions.Like(t.Name.ToLower(), $"%{findString}%") 
                        || EF.Functions.Like(t.LocalNumber.LocalNumber.ToString(), $"%{findString}%"))
                     .OrderBy(t => t.Name)
                     .ToList());
                }
            }
            // Сделать условия по склонению
            CountUser.Content = $"В базе данных найдено {Users.Count} записей";
        }
        public MainWindow()
        {
            // Users.CollectionChanged += Users_CollectionChanged;
            InitializeComponent();
            CommonNumber.Content = "Общий многоканальный номер 344 - 154";
            CheckOnline.SetStatus();
            GetData("");
            MainTable.ItemsSource = Users;
        }

        // Закрытие текущего кона приложения
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

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            EditUserPage newUserPage = new EditUserPage(null);
            newUserPage.ShowDialog();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var editUser = (User)MainTable.SelectedItem;
            if (editUser != null)
            {
                if (!editUser.Common)
                {
                    EditUserPage editUserPage = new EditUserPage(editUser);
                    editUserPage.ShowDialog();
                }
                else
                {
                    EditCommonPage editCommonPage = new EditCommonPage(editUser);
                    editCommonPage.ShowDialog();
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var deleteUser = (User)MainTable.SelectedItem;
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
                        Users.Remove(deleteUser);
                        db.Users.Remove(deleteUser);
                        db.SaveChanges();
                    }
                }
            }
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            var findString = FindString.Text;
            GetData(findString);
            MainTable.ItemsSource = Users;
        }

        private void FindString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var findString = FindString.Text;
                GetData(findString);
                MainTable.ItemsSource = Users;
            }
        }

        private void NewCommon_Click(object sender, RoutedEventArgs e)
        {
            EditCommonPage newCommonPage = new EditCommonPage(null);
            newCommonPage.ShowDialog();
        }
    }
}
