using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Phone_Book
{
    public partial class EditUserPage : Window
    {
        User insertUser = null;
        DbContextOptions<ApplicationContext> options;
        // Основной метод страницы
        public EditUserPage(User currentUser)
        {
            // Подключение к базе данных из файла
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var opetionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            options = opetionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            InitializeComponent();
            InsertDataInComboBox(currentUser);
        }

        // Метод для заполнения поле данными при загрузке
        private void InsertDataInComboBox(User user)
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                ComboBoxDepartment.ItemsSource = db.Deparments.ToList();
                ComboBoxDepartment.DisplayMemberPath = "DepartmentName";

                ComboBoxLocal.ItemsSource = db.Locals.ToList();
                ComboBoxLocal.DisplayMemberPath = "LocalNumber";

                ComboBoxCity.ItemsSource = db.Cities.ToList();
                ComboBoxCity.DisplayMemberPath = "CityNumber";

                if (user != null)
                {
                    insertUser = user;

                    string[] nameUser = user.Name.Split(' ');
                    TextBoxSurname.Text = nameUser[0];
                    TextBoxName.Text = nameUser[1];
                    TextBoxMiddleName.Text = nameUser[2];

                    TextBoxPosition.Text = user.Positon;
                    TextBoxMobile.Text = user.MobileNumber.ToString();
                    TextBoxAbsense.Text = user.Absense;

                    if (user.Department != null)
                        ComboBoxDepartment.SelectedIndex = db.Deparments.ToList().FindIndex(t => t.DepartmentName.Equals(user.Department.DepartmentName));

                    if (user.LocalNumber != null)
                        ComboBoxLocal.SelectedIndex = db.Locals.ToList().FindIndex(t => t.LocalNumber.Equals(user.LocalNumber.LocalNumber));

                    if (user.CityNumber != null)
                        ComboBoxCity.SelectedIndex = db.Cities.ToList().FindIndex(t => t.CityNumber.Equals(user.CityNumber.CityNumber));
                }
            }
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Topmost = true;
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите сохранять изменения?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Topmost = false;

                if (insertUser == null)
                {
                    CreatyNewUser();
                    this.Close();
                }
                else
                {
                    EditCurrentUser();
                    this.Close();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Topmost = true;
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите не сохранять изменения?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                Topmost = false;
                this.Close();
            }
        }
        // Макрос для создания новых записей в базе данных, при условии, что таковых значений в базе нет
        private object CreatyStringInTable(object insertObject)
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                switch (insertObject)
                {
                    case Department department:
                        if (ComboBoxDepartment.SelectedIndex == -1)
                        {
                            department = new Department { DepartmentName = ComboBoxDepartment.Text };
                            db.Deparments.Add(department);
                            db.SaveChanges();
                        }
                        else
                        {
                            department = (Department)ComboBoxDepartment.SelectedItem;
                        }
                        return department;

                    case Local local:
                        if (ComboBoxLocal.SelectedIndex == -1)
                        {
                            local = new Local { LocalNumber = Int32.Parse(ComboBoxLocal.Text) };
                            db.Locals.Add(local);
                            db.SaveChanges();
                            insertUser.LocalNumber = local;
                        }
                        else
                        {
                            local = (Local)ComboBoxLocal.SelectedItem;
                        }
                        return local;

                    case City city:
                        if (ComboBoxCity.SelectedIndex == -1)
                        {
                            city = new City { CityNumber = Int32.Parse(ComboBoxCity.Text) };
                            db.Cities.Add(city);
                            db.SaveChanges();
                            insertUser.CityNumber = city;
                        }
                        else
                        {
                            city = (City)ComboBoxCity.SelectedItem;
                        }
                        return city;
                    default:
                        throw new Exception();
                }
            }
        }
        // Метод для внесения изменений при сохранении изменений у пользователя
        private void EditCurrentUser()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                insertUser.Name = nameUser;
                insertUser.Positon = TextBoxPosition.Text;
                insertUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);
                insertUser.Department = (Department)CreatyStringInTable(new Department());
                insertUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                insertUser.CityNumber = (City)CreatyStringInTable(new City());
                insertUser.Absense = TextBoxAbsense.Text;
                db.Entry(insertUser).State = EntityState.Modified;
                db.SaveChanges();
                MessageBox.Show("Информация о пользователе обновлена");
            }
        }
        private void CreatyNewUser()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                User newUser = new User();
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                newUser.Name = nameUser;
                newUser.Positon = TextBoxPosition.Text;

                if (ComboBoxDepartment.SelectedIndex == -1)
                {
                    var l1 = new Department { DepartmentName = ComboBoxLocal.Text };
                    db.Deparments.Add(l1);
                    db.SaveChanges();
                    newUser.Department = l1;
                }
                else
                {
                    newUser.Department = (Department)ComboBoxDepartment.SelectedItem;
                }

                if (ComboBoxLocal.SelectedIndex == -1)
                {
                    var l1 = new Local { LocalNumber = Int32.Parse(ComboBoxLocal.Text) };
                    db.Locals.Add(l1);
                    db.SaveChanges();
                    newUser.LocalNumber = l1;
                }
                else
                {
                    newUser.LocalNumber = (Local)ComboBoxLocal.SelectedItem;
                }

                if (ComboBoxCity.SelectedIndex == -1)
                {
                    var l1 = new City { CityNumber = Int32.Parse(ComboBoxCity.Text) };
                    db.Cities.Add(l1);
                    db.SaveChanges();
                    newUser.CityNumber = l1;
                }
                else
                {
                    newUser.CityNumber = (City)ComboBoxCity.SelectedItem;
                }
                newUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);
                newUser.Absense = TextBoxAbsense.Text;
                db.Entry(newUser).State = EntityState.Added;
                db.SaveChanges();
                MessageBox.Show("Новый сотрудник организации добавлен!");
            }
        }
    }
}
