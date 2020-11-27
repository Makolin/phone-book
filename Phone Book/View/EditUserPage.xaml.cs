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
        public EditUserPage(User currentUser)
        {
            InitializeComponent();
            InsertDataInComboBox(currentUser);
        }

        // Для заполнения полей данными у выбранного пользователя из базы данных
        private void InsertDataInComboBox(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
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

                    TextBoxPosition.Text = user.Position;
                    TextBoxMobile.Text = user.MobileNumber.ToString();
                    TextBoxAbsense.Text = user.Absence;

                    if (user.Department != null)
                        ComboBoxDepartment.SelectedIndex = db.Deparments.ToList().FindIndex(t => t.DepartmentName.Equals(user.Department.DepartmentName));

                    if (user.LocalNumber != null)
                        ComboBoxLocal.SelectedIndex = db.Locals.ToList().FindIndex(t => t.LocalNumber.Equals(user.LocalNumber.LocalNumber));

                    if (user.CityNumber != null)
                        ComboBoxCity.SelectedIndex = db.Cities.ToList().FindIndex(t => t.CityNumber.Equals(user.CityNumber.CityNumber));
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
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                insertUser.Name = nameUser;
                insertUser.Position = TextBoxPosition.Text;

                if (TextBoxMobile.Text != string.Empty)
                    insertUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);

                insertUser.Department = (Department)CreatyStringInTable(new Department());
                if (insertUser.Department != null)
                    insertUser.DepartmentId = insertUser.Department.DepartmentId;

                insertUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                if (insertUser.LocalNumber != null)
                    insertUser.LocalId = insertUser.LocalNumber.LocalId;

                insertUser.CityNumber = (City)CreatyStringInTable(new City());
                if(insertUser.CityNumber != null)
                    insertUser.CityId = insertUser.CityNumber.CityId;

                insertUser.Absence = TextBoxAbsense.Text;

                db.Entry(insertUser).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        private void CreatyNewUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User newUser = new User();
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                newUser.Name = nameUser;
                newUser.Position = TextBoxPosition.Text;

                newUser.Department = (Department)CreatyStringInTable(new Department());
                newUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                newUser.CityNumber = (City)CreatyStringInTable(new City());

                if (TextBoxMobile.Text != string.Empty)
                    newUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);

                newUser.Absence = TextBoxAbsense.Text;
                db.Entry(newUser).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
