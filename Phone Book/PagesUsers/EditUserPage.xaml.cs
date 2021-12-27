using Microsoft.EntityFrameworkCore;
using Phone_Book.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Phone_Book.Pages
{
    public partial class EditUserWindow : Window
    {
        private User insertUser;
        public EditUserWindow(User currentUser)
        {
            InitializeComponent();
            InsertDataInComboBox(currentUser);
        }

        // Для заполнения полей данными у выбранного пользователя из базы данных
        private void InsertDataInComboBox(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var PositionList = db.Positions.OrderBy(t => t.PositionName).ToList();
                ComboBoxPosition.ItemsSource = PositionList;
                ComboBoxPosition.DisplayMemberPath = "PositionName";

                var DepartmentList = db.Deparments.OrderBy(t => t.DepartmentFullName).ToList();
                ComboBoxDepartment.ItemsSource = DepartmentList;
                ComboBoxDepartment.DisplayMemberPath = "DepartmentFullName";

                var LocalList = db.Locals.OrderBy(t => t.LocalNumber).ToList();
                ComboBoxLocal.ItemsSource = LocalList;
                ComboBoxLocal.DisplayMemberPath = "LocalNumber";

                var CityList = db.Cities.OrderBy(t => t.CityNumber).ToList();
                ComboBoxCity.ItemsSource = CityList;
                ComboBoxCity.DisplayMemberPath = "CityNumber";

                var ComputerNameList = db.Computers.OrderBy(t => t.ComputerId).ToList();
                ComboBoxNameComputer.ItemsSource = ComputerNameList;
                ComboBoxNameComputer.DisplayMemberPath = "NameComputer";

                if (user != null)
                {
                    insertUser = user;

                    string[] nameUser = user.Name.Split(' ');
                    TextBoxSurname.Text = nameUser[0];
                    TextBoxName.Text = nameUser[1];
                    TextBoxMiddleName.Text = nameUser[2];

                    if (user.Birthday != default)
                    {
                        DatePickerBirhday.SelectedDate = user.Birthday;
                    }

                    if (user.MobileNumber != null)
                    {
                        TextBoxMobile.Text = user.MobileNumber.ToString();
                    }

                    if (user.DomainName != null)
                    {
                        TextBoxDomainName.Text = user.DomainName.ToString();
                    }

                    if (user.Position != null)
                    {
                        ComboBoxPosition.SelectedIndex = PositionList.FindIndex(t => t.PositionId == user.PositionId);
                    }

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

                    if (user.ComputerStatus != null)
                    {
                        ComboBoxNameComputer.SelectedIndex = ComputerNameList.FindIndex(t => t.ComputerId == user.ComputerId);
                    }
                }
            }
        }

        // Проверка ввода значений в полях для ввода
        private bool CheckTextInsert()
        {
            bool hasMistake = false;

            TextBoxSurnameError.Content = string.Empty;
            TextBoxNameError.Content = string.Empty;
            TextBoxMiddleNameError.Content = string.Empty;
            TextBoxDataBirthdayError.Content = string.Empty;

            ComboBoxPositionError.Content = string.Empty;
            ComboBoxDepartmentError.Content = string.Empty;

            ComboBoxLocalError.Content = string.Empty;
            ComboBoxCityError.Content = string.Empty;
            TextBoxMobileError.Content = string.Empty;

            ComboBoxNameComputerError.Content = string.Empty;

            if (TextBoxSurname.Text == string.Empty)
            {
                TextBoxSurnameError.Content = "Обязательное поле";
                hasMistake = true;
            }

            if (TextBoxName.Text == string.Empty)
            {
                TextBoxNameError.Content = "Обязательное поле";
                hasMistake = true;
            }

            if (TextBoxMiddleName.Text == string.Empty)
            {
                TextBoxMiddleNameError.Content = "Обязательное поле";
                hasMistake = true;
            }

            if (DatePickerBirhday.SelectedDate != default)
            {
                if (DatePickerBirhday.SelectedDate > DateTime.Today)
                {
                    TextBoxDataBirthdayError.Content = "Дата рождения не может быть в будущем";
                    hasMistake = true;
                }
                else if (DatePickerBirhday.SelectedDate > DateTime.Today.AddYears(-18))
                {
                    TextBoxDataBirthdayError.Content = "Дата рождения не может быть раньше 18 лет назад";
                    hasMistake = true;
                }
            }

            if (string.IsNullOrEmpty(ComboBoxPosition.Text))
            {
                ComboBoxPositionError.Content = "Обязательное поле";
                hasMistake = true;
            }

            if (string.IsNullOrEmpty(ComboBoxDepartment.Text))
            {
                ComboBoxDepartmentError.Content = "Обязательное поле";
                hasMistake = true;
            }

            if (string.IsNullOrEmpty(ComboBoxLocal.Text))
            {
                ComboBoxLocalError.Content = "Обязательное поле";
                hasMistake = true;
            }
            else if (ComboBoxLocal.Text.Length != 3)
            {
                ComboBoxLocalError.Content = "Длина местного номера 3 цифры";
                hasMistake = true;
            }

            if (ComboBoxCity.Text.Length is not 0 and not 6)
            {
                ComboBoxCityError.Content = "Длина городского номера 6 цифр";
                hasMistake = true;
            }

            if (TextBoxMobile.Text != string.Empty)
            {
                if (TextBoxMobile.Text.Length != 11)
                {
                    TextBoxMobileError.Content = "Длина мобильного номера 11 цифр";
                    hasMistake = true;
                }
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
                        if (CreateNewUser())
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
                    case Position position:
                        position = (Position)ComboBoxPosition.SelectedItem;
                        return position;

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

                    case Computer online:
                        if (ComboBoxNameComputer.SelectedIndex == -1 && !string.IsNullOrEmpty(ComboBoxNameComputer.Text))
                        {
                            online = new Computer { NameComputer = ComboBoxNameComputer.Text };
                            db.Computers.Add(online);
                            db.SaveChanges();
                        }
                        else
                        {
                            online = (Computer)ComboBoxNameComputer.SelectedItem;
                        }
                        return online;

                    default:
                        throw new Exception();
                }
            }
        }

        // Для внесения изменений для редактируемого пользователя
        private bool EditCurrentUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                var findUser = db.Users.Any(t => t.Name.ToLower() == nameUser.ToLower().Trim() && t.UserId != insertUser.UserId);

                if (findUser == false)
                {
                    UserCollection.Users.Remove(insertUser);

                    insertUser.Name = nameUser;

                    if (!string.IsNullOrEmpty(TextBoxMobile.Text))
                    {
                        insertUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);
                    }

                    if (DatePickerBirhday.SelectedDate != default)
                    {
                        insertUser.Birthday = (DateTime)DatePickerBirhday.SelectedDate;
                    }

                    if (!string.IsNullOrEmpty(TextBoxDomainName.Text))
                    {
                        insertUser.DomainName = TextBoxDomainName.Text;
                    }

                    insertUser.Position = (Position)CreatyStringInTable(new Position());
                    insertUser.PositionId = insertUser.Position?.PositionId;

                    insertUser.Department = (Department)CreatyStringInTable(new Department());
                    insertUser.DepartmentId = insertUser.Department?.DepartmentId;

                    insertUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                    insertUser.LocalId = insertUser.LocalNumber?.LocalId;

                    insertUser.CityNumber = (City)CreatyStringInTable(new City());
                    insertUser.CityId = insertUser.CityNumber?.CityId;

                    insertUser.ComputerStatus = (Computer)CreatyStringInTable(new Computer());
                    insertUser.ComputerId = insertUser.ComputerStatus?.ComputerId;

                    UserCollection.Users.Add(insertUser);
                    db.Entry(insertUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    MessageBox.Show("Пользователь с данными ФИО уже существует в справочнике");
                    return false;
                }
            }
        }

        // Создание нового пользователя
        private bool CreateNewUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";

                var findUser = db.Users.Any(t => t.Name.ToLower() == nameUser.ToLower().Trim());

                if (findUser == false)
                {
                    User newUser = new User()
                    {
                        Name = nameUser,
                        Position = (Position)CreatyStringInTable(new Position()),
                        Department = (Department)CreatyStringInTable(new Department()),
                        LocalNumber = (Local)CreatyStringInTable(new Local()),
                        CityNumber = (City)CreatyStringInTable(new City()),
                        ComputerStatus = (Computer)CreatyStringInTable(new Computer())
                    };

                    if (!string.IsNullOrEmpty(TextBoxMobile.Text))
                    {
                        newUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);
                    }

                    if (DatePickerBirhday.SelectedDate != default)
                    {
                        newUser.Birthday = (DateTime)DatePickerBirhday.SelectedDate;
                    }

                    if (!string.IsNullOrEmpty(TextBoxDomainName.Text))
                    {
                        newUser.DomainName = TextBoxDomainName.Text;
                    }

                    UserCollection.Users.Add(newUser);
                    db.Entry(newUser).State = EntityState.Added;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    MessageBox.Show("Пользователь с данными ФИО уже существует в справочнике");
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

        private void ClearPosition_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxPosition.SelectedItem = null;
        }
    }
}
