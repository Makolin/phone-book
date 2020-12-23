﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Windows;

namespace Phone_Book
{
    public partial class EditUserPage : Window
    {
        User insertUser;
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
                var PositionList = db.Positions.OrderBy(t => t.PositionName).ToList();
                ComboBoxPosition.ItemsSource = PositionList;
                ComboBoxPosition.DisplayMemberPath = "PositionName";

                var DepartmentList = db.Deparments.OrderBy(t => t.DepartmentName).ToList();
                ComboBoxDepartment.ItemsSource = DepartmentList;
                ComboBoxDepartment.DisplayMemberPath = "DepartmentName";

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

                    TextBoxMobile.Text = user.MobileNumber.ToString();
                    TextBoxAbsense.Text = user.Absence;

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

        // Для подтверждения сохранения внесенных изменений или создания нового пользователя
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите сохранять изменения?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (insertUser == null) CreatyNewUser();
                else EditCurrentUser();
                this.Close();
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
        // Метод для внесения изменений при сохранении изменений у пользователя
        private void EditCurrentUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string nameUser = $"{TextBoxSurname.Text} {TextBoxName.Text} {TextBoxMiddleName.Text}";
                insertUser.Name = nameUser;

                if (!string.IsNullOrEmpty(TextBoxMobile.Text))
                    insertUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);

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

                newUser.Position = (Position)CreatyStringInTable(new Position());
                newUser.Department = (Department)CreatyStringInTable(new Department());
                newUser.LocalNumber = (Local)CreatyStringInTable(new Local());
                newUser.CityNumber = (City)CreatyStringInTable(new City());
                newUser.ComputerStatus = (Computer)CreatyStringInTable(new Computer());

                if (!string.IsNullOrEmpty(TextBoxMobile.Text))
                    newUser.MobileNumber = Convert.ToInt64(TextBoxMobile.Text);

                newUser.Absence = TextBoxAbsense.Text;
                db.Entry(newUser).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
