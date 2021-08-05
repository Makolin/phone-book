using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Phone_Book.Model;

namespace Phone_Book.Pages
{
    public partial class EditUserWindow : Window
    {
        User insertUser;
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

                    TextBoxDataBirthday.Text = user.Birthday.ToShortDateString();
                    TextBoxMobile.Text = user.MobileNumber.ToString();

                    if (user.Position != null)
                        ComboBoxPosition.SelectedIndex = PositionList.FindIndex(t => t.PositionId == user.PositionId);

                    if (user.Department != null)
                        ComboBoxDepartment.SelectedIndex = DepartmentList.FindIndex(t => t.DepartmentId == user.DepartmentId);

                    if (user.LocalNumber != null)
                        ComboBoxLocal.SelectedIndex = LocalList.FindIndex(t => t.LocalId == user.LocalId);

                    if (user.CityNumber != null)
                        ComboBoxCity.SelectedIndex = CityList.FindIndex(t => t.CityId == user.CityId);

                    if (user.ComputerStatus != null)
                        ComboBoxNameComputer.SelectedIndex = ComputerNameList.FindIndex(t => t.ComputerId == user.ComputerId);
                }
            }
        }

        // Проверка ввода значений в полях для ввода
        // Не окрашивает ComboBox
        private bool CheckTextInsert()
        {
            var hasMistake = false;
            if (TextBoxSurname.Text == string.Empty)
            {
                TextBoxSurname.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else TextBoxSurname.Background = Brushes.LightGreen;

            if (TextBoxName.Text == string.Empty)
            {
                TextBoxName.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else TextBoxName.Background = Brushes.LightGreen;

            if (TextBoxMiddleName.Text == string.Empty)
            {
                TextBoxMiddleName.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else TextBoxMiddleName.Background = Brushes.LightGreen;

            // Дата рождения
            DateTime dateValue;
            if (TextBoxDataBirthday.Text != string.Empty && !DateTime.TryParse(TextBoxDataBirthday.Text, out dateValue))
            {
                TextBoxDataBirthday.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else TextBoxDataBirthday.Background = Brushes.LightGreen;

            if (string.IsNullOrEmpty(ComboBoxPosition.Text))
            {
                ComboBoxPosition.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else ComboBoxPosition.Background = Brushes.LightGreen;

            if (string.IsNullOrEmpty(ComboBoxDepartment.Text))
            {
                ComboBoxDepartment.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else ComboBoxDepartment.Background = Brushes.LightGreen;

            if (ComboBoxLocal.Text.Length != 3)
            {
                ComboBoxLocal.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else ComboBoxLocal.Background = Brushes.LightGreen;

            if (ComboBoxCity.Text.Length != 0 && ComboBoxCity.Text.Length != 6)
            {
                ComboBoxCity.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else ComboBoxCity.Background = Brushes.LightGreen;

            if (TextBoxMobile.Text.Length != 0 && TextBoxMobile.Text.Length != 11)
            {
                TextBoxMobile.Background = Brushes.LightCoral;
                hasMistake = true;
            }
            else TextBoxMobile.Background = Brushes.LightGreen;
            {

            }

            if (hasMistake) return false;
            else return true;
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
                    if (insertUser == null) CreateNewUser();
                    else EditCurrentUser();
                    this.Close();
                }
                else
                {
                    this.Close();
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

            if (result == MessageBoxResult.Yes) this.Close();
        }

        // Макрос для создания новых записей в базе данных, если значение было добавлено пользователем и не существует в базе данных
        private object CreatyStringInTable(object insertObject)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                switch (insertObject)
                {
                    case Position position:
                        if (ComboBoxPosition.SelectedIndex == -1 && !string.IsNullOrEmpty(ComboBoxPosition.Text))
                        {
                            position = new Position { PositionName = ComboBoxPosition.Text };
                            db.Positions.Add(position);
                            db.SaveChanges();
                        }
                        else
                        {
                            position = (Position)ComboBoxPosition.SelectedItem;
                        }
                        return position;

                    case Department department:
                        if (ComboBoxDepartment.SelectedIndex == -1 && !string.IsNullOrEmpty(ComboBoxDepartment.Text))
                        {
                            department = new Department { DepartmentFullName = ComboBoxDepartment.Text };
                            db.Deparments.Add(department);
                            db.SaveChanges();
                        }
                        else
                        {
                            department = (Department)ComboBoxDepartment.SelectedItem;
                        }
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
        private void EditCurrentUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                UserCollection.Users.Remove(insertUser);

                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                insertUser.Name = nameUser;

                if (!string.IsNullOrEmpty(TextBoxMobile.Text))
                    insertUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);

                if (!string.IsNullOrEmpty(TextBoxDataBirthday.Text))
                    insertUser.Birthday = DateTime.Parse(TextBoxDataBirthday.Text);

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
            }
        }

        // Создание нового пользователя
        private void CreateNewUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User newUser = new User();
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                newUser.Name = nameUser;

                newUser.Position = (Position)CreatyStringInTable(new Position());
                newUser.Department = (Department)CreatyStringInTable(new Department());
                newUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                newUser.CityNumber = (City)CreatyStringInTable(new City());
                newUser.ComputerStatus = (Computer)CreatyStringInTable(new Computer());

                if (!string.IsNullOrEmpty(TextBoxMobile.Text))
                    newUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);

                if (!string.IsNullOrEmpty(TextBoxDataBirthday.Text))
                    insertUser.Birthday = DateTime.Parse(TextBoxDataBirthday.Text);

                UserCollection.Users.Add(newUser);
                db.Entry(newUser).State = EntityState.Added;
                db.SaveChanges();
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
