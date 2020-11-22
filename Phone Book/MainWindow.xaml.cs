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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<User> GetData()
        {
            ObservableCollection<User> taskUser = null; ;
            using (ApplicationContext db = new ApplicationContext())
            {
                taskUser = new ObservableCollection<User>(db.Users.ToList());
            }

            return taskUser;
        }
        public MainWindow()
        {
            // Подключение к базе данных из файла
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var opetionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = opetionsBuilder
                .UseSqlServer(connectionString)
                .Options;
            
            // Загрузка первичных данных при создании базы
            using (var context = new ApplicationContext())
            {
                /*var department = new Department { DepartmentName = "Отдел №12" };
                context.Deparments.Add(department);

                var localNumber = new Local { LocalNumber = 238 };
                context.Locals.Add(localNumber);

                var cityNumber = new City { CityNumber = 344154 };
                context.Cities.Add(cityNumber);

                var user = new User
                {
                    Name = "Петров Петр Петрович",
                    Department = department,
                    Positon = "Начальник отдела",
                    LocalNumber = localNumber,
                    CityNumber = cityNumber,
                    MobileNumber = 89099099090,
                    Absense = "Отпуск по 16.11.2020"
                };
                context.Users.Add(user);

                context.SaveChanges();*/
                //var user = context.Users.Include(u => u.Department).ToList();
                //MainTable.ItemsSource = user;
            }

            InitializeComponent();
            ObservableCollection<User> taskUser = GetData();
            MainTable.DataContext = taskUser;
        }

        private void MainTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*User selectedUser = (User)MainTable.SelectedItem;
            EditUserPage editUser = new EditUserPage(context, selectedUser);
            editUser.Show();*/
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вывод информации о текущей версии приложения.");
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new ApplicationContext())
            {
                var findString = FindString.Text;
                if (findString == "")
                {
                    context.Users.Load();
                    MainTable.ItemsSource = context.Users.Local.ToBindingList();
                }
                else
                {
                    findString = findString.ToLower().Trim();
                    context.Users.Load();
                    MainTable.ItemsSource = context.Users.Local.Where(p => p.Name.ToLower().Contains(findString)).ToList();
                }
            }

        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            /*EditUserPage newUser = new EditUserPage(context, null);
            newUser.Show();*/
        }
    }
}
