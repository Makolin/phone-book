using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace Phone_Book.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditCommonPage.xaml
    /// </summary>
    public partial class EditCommonPage : Window
    {
        User insertUser = null;
        public EditCommonPage(User currentUser)
        {
            InitializeComponent();
            InsertDataInComboBox(currentUser);
        }

        // Для заполнения полей данными у выбранного пользователя из базы данных
        private void InsertDataInComboBox(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                var DepartmentList = db.Deparments.OrderBy(t => t.DepartmentName).ToList();
                ComboBoxDepartment.ItemsSource = DepartmentList;
                ComboBoxDepartment.DisplayMemberPath = "DepartmentName";

                var LocalList = db.Locals.OrderBy(t => t.LocalNumber).ToList();
                ComboBoxLocal.ItemsSource = LocalList;
                ComboBoxLocal.DisplayMemberPath = "LocalNumber";

                var CityList = db.Cities.OrderBy(t => t.CityNumber).ToList();
                ComboBoxCity.ItemsSource = CityList;
                ComboBoxCity.DisplayMemberPath = "CityNumber";

                if (user != null)
                {
                    insertUser = user;

                    TextBoxSurname.Text = user.Name;

                    if (user.Department != null)
                        ComboBoxDepartment.SelectedIndex = DepartmentList.FindIndex(t => t.DepartmentId == user.DepartmentId);

                    if (user.LocalNumber != null)
                        ComboBoxLocal.SelectedIndex = LocalList.FindIndex(t => t.LocalId == user.LocalId);

                    if (user.CityNumber != null)
                        ComboBoxCity.SelectedIndex = CityList.FindIndex(t => t.CityId == user.CityId);
                }
            }
        }

        // Для подтверждения сохранения внесенных изменений или создания нового пользователя
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

        // Для подтверждения отмены сохранения изменений для текущего пользователя
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

        // Макрос для создания новых записей в базе данных, если значение было добавлено пользователем и не существует в базе данных
        private object CreatyStringInTable(object insertObject)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                switch (insertObject)
                {
                    case Department department:
                        if (ComboBoxDepartment.SelectedIndex == -1 && ComboBoxDepartment.Text != string.Empty)
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
                        if (ComboBoxLocal.SelectedIndex == -1 && ComboBoxLocal.Text != string.Empty)
                        {
                            local = new Local { LocalNumber = Int32.Parse(ComboBoxLocal.Text) };
                            db.Locals.Add(local);
                            db.SaveChanges();
                        }
                        else
                        {
                            local = (Local)ComboBoxLocal.SelectedItem;
                        }
                        return local;

                    case City city:
                        if (ComboBoxCity.SelectedIndex == -1 && ComboBoxCity.Text != string.Empty)
                        {
                            city = new City { CityNumber = Int32.Parse(ComboBoxCity.Text) };
                            db.Cities.Add(city);
                            db.SaveChanges();
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
            using (ApplicationContext db = new ApplicationContext())
            {
                insertUser.Name = TextBoxSurname.Text;

                insertUser.Department = (Department)CreatyStringInTable(new Department());
                if (insertUser.Department != null)
                    insertUser.DepartmentId = insertUser.Department.DepartmentId;

                insertUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                if (insertUser.LocalNumber != null)
                    insertUser.LocalId = insertUser.LocalNumber.LocalId;

                insertUser.CityNumber = (City)CreatyStringInTable(new City());
                if (insertUser.CityNumber != null)
                    insertUser.CityId = insertUser.CityNumber.CityId;

                db.Entry(insertUser).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        private void CreatyNewUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User newUser = new User();
                string nameUser = TextBoxSurname.Text;
                newUser.Name = nameUser;
                newUser.Common = true;

                newUser.Department = (Department)CreatyStringInTable(new Department());
                newUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                newUser.CityNumber = (City)CreatyStringInTable(new City());

                db.Entry(newUser).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
