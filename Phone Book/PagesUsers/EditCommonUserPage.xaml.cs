using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Phone_Book.Model;
using System.Windows.Media;

namespace Phone_Book.Pages
{
    public partial class EditCommonUserWindow : Window
    {
        private User insertUser;
        public EditCommonUserWindow(User currentUser)
        {
            InitializeComponent();
            InsertDataInComboBox(currentUser);
        }

        // Для заполнения полей данными у выбранного пользователя из базы данных
        private void InsertDataInComboBox(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var DepartmentList = db.Deparments.OrderBy(t => t.DepartmentFullName).ToList();
                ComboBoxDepartment.ItemsSource = DepartmentList;
                ComboBoxDepartment.DisplayMemberPath = "DepartmentFullName";

                var LocalList = db.Locals.OrderBy(t => t.LocalNumber).ToList();
                ComboBoxLocal.ItemsSource = LocalList;
                ComboBoxLocal.DisplayMemberPath = "LocalNumber";

                var CityList = db.Cities.OrderBy(t => t.CityNumber).ToList();
                ComboBoxCity.ItemsSource = CityList;
                ComboBoxCity.DisplayMemberPath = "CityNumber";

                if (user != null)
                {
                    insertUser = user;

                    TextBoxAbonent.Text = user.Name;

                    if (user.Department != null)
                    {
                        ComboBoxDepartment.SelectedIndex = DepartmentList.FindIndex(t => t.DepartmentId == user.DepartmentId);
                    }

                    if (user.LocalNumber != null)
                    {
                        ComboBoxLocal.SelectedIndex = LocalList.FindIndex(t => t.LocalId == user.LocalId);
                    }

                    if (user.CityNumber != null)
                    {
                        ComboBoxCity.SelectedIndex = CityList.FindIndex(t => t.CityId == user.CityId);
                    }
                }
            }
        }

        // Проверка на заполнение обязательного поля
        private bool CheckTextInsert()
        {
            bool hasMistake = false;
            TextBoxAbonentError.Content = string.Empty;
            ComboBoxDepartmentError.Content = string.Empty;
            ComboBoxLocalError.Content = string.Empty;
            ComboBoxCityError.Content = string.Empty;


            if (TextBoxAbonent.Text == string.Empty)
            {
                TextBoxAbonentError.Content = "Обязательное поле";
                hasMistake = true;
            }

            if (string.IsNullOrEmpty(ComboBoxDepartment.Text))
            {
                ComboBoxDepartmentError.Content = "Обязательное поле";
                hasMistake = true;
            }

            if (ComboBoxLocal.Text.Length == 0)
            {
                ComboBoxLocalError.Content = "Обязательное поле";
                hasMistake = true;
            }
            else if (ComboBoxLocal.Text.Length != 3)
            {
                ComboBoxLocalError.Content = "Длина местного номера 3 цифры";
                hasMistake = true;
            }

            if (ComboBoxCity.Text.Length != 0 && ComboBoxCity.Text.Length != 6)
            {
                ComboBoxCityError.Content = "Длина городского номера 6 цифр";
                hasMistake = true;
            }

            return !hasMistake;
        }

        // Для подтверждения сохранения внесенных изменений или создания нового пользователя
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (CheckTextInsert())
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы действительно хотите сохранять изменения?",
                    "Сохранение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (insertUser == null)
                    {
                        if (CreatyNewUser())
                        {
                            Close();
                        }
                    }
                    else
                    {
                        if (EditCurrentUser())
                        {
                            Close();
                        }
                    }
                }
                else
                {
                    Close();
                }
            }
        }

        // Для подтверждения отмены сохранения изменений для текущего пользователя
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите не сохранять изменения?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                Close();
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
                        department = (Department)ComboBoxDepartment.SelectedItem;
                        return department;

                    case Local local:
                        if (ComboBoxLocal.SelectedIndex == -1 && !string.IsNullOrEmpty(ComboBoxLocal.Text))
                        {
                            local = new Local { LocalNumber = int.Parse(ComboBoxLocal.Text) };
                            db.Locals.Add(local);
                            db.SaveChanges();
                        }
                        else
                        {
                            local = (Local)ComboBoxLocal.SelectedItem;
                        }
                        return local;

                    case City city:
                        if (ComboBoxCity.SelectedIndex == -1 && !string.IsNullOrEmpty(ComboBoxCity.Text))
                        {
                            city = new City { CityNumber = int.Parse(ComboBoxCity.Text) };
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
        private bool EditCurrentUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string nameUser = TextBoxAbonent.Text;
                var findUser = db.Users.Any(t => t.Name.ToLower() == nameUser.ToLower().Trim() && t.UserId != insertUser.UserId);

                if (findUser == false)
                {
                    insertUser.Name = TextBoxAbonent.Text;

                    insertUser.Department = (Department)CreatyStringInTable(new Department());
                    insertUser.DepartmentId = insertUser.Department?.DepartmentId;

                    insertUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                    insertUser.LocalId = insertUser.LocalNumber?.LocalId;

                    insertUser.CityNumber = (City)CreatyStringInTable(new City());
                    insertUser.CityId = insertUser.CityNumber?.CityId;

                    db.Entry(insertUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    MessageBox.Show("Абонент с данным наименованием уже существует в справочнике");
                    return false;
                }
            }
        }

        // Создание нового пользователя
        private bool CreatyNewUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string nameUser = TextBoxAbonent.Text;
                var findUser = db.Users.Any(t => t.Name.ToLower() == nameUser.ToLower().Trim());

                if (findUser == false)
                {
                    User newUser = new User()
                    {
                        Name = nameUser,
                        Common = true,
                        Department = (Department)CreatyStringInTable(new Department()),
                        LocalNumber = (Local)CreatyStringInTable(new Local()),
                        CityNumber = (City)CreatyStringInTable(new City())
                    };

                    UserCollection.Users.Add(newUser);
                    db.Entry(newUser).State = EntityState.Added;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    MessageBox.Show("Абонент с данным наименованием уже существует в справочнике");
                    return false;
                }     
            }
        }

        // Проверка на ввод только чисел
        private void ValidatonOnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Проверка на ввод только кириллицы 
        private void ValidatonOnlyText(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Очистка combobox при неверном выборе значения
        private void ClearDepartment_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxDepartment.SelectedItem = null;
        }
    }
}