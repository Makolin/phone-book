using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Phone_Book.PagesDepartments
{
    public partial class EditDepartmentWindow : Window
    {
        private Department insertDepartment;
        public EditDepartmentWindow(Department currentDepartment)
        {
            InitializeComponent();
            InsertDataInComboBox(currentDepartment);
        }

        private void InsertDataInComboBox(Department department)
        {
            if (department != null)
            {
                insertDepartment = department;
                TextBoxNumber.Text = department.DepartmentNumber.ToString();
                TextBoxFullName.Text = department.DepartmentFullName;
                TextBoxShortName.Text = department.DepartmentShortName;
            }
        }

        // Создание новой должности
        private void CreatyNewDepartment()
        {
            using (ApplicationContext db = new())
            {
                Department newDepartment = new Department
                {
                    DepartmentNumber = Convert.ToInt32(TextBoxNumber.Text),
                    DepartmentFullName = TextBoxFullName.Text,
                    DepartmentShortName = TextBoxShortName.Text
                };

                DepartmentPage.Departments.Add(newDepartment);
                db.Entry(newDepartment).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        // Редактирование выбранного подразделения
        private void EditCurrentDepartment()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                DepartmentPage.Departments.Remove(insertDepartment);
                insertDepartment.DepartmentNumber = Convert.ToInt32(TextBoxNumber.Text);
                insertDepartment.DepartmentFullName = TextBoxFullName.Text;
                insertDepartment.DepartmentShortName = TextBoxShortName.Text;

                db.Entry(insertDepartment).State = EntityState.Modified;
                db.SaveChanges();
                DepartmentPage.Departments.Add(insertDepartment);
            }
        }

        // Проверка на заполнение обязательного поля
        // Проверка на уникальность 
        private bool CheckTextInsert()
        {
            bool hasMistake = false;
            TextBoxNumberError.Content = string.Empty;
            TextBoxFullNameError.Content = string.Empty;
            TextBoxShortNameError.Content = string.Empty;

            if (TextBoxNumber.Text == string.Empty)
            {
                hasMistake = true;
                TextBoxNumberError.Content = "Обязательное поле";
            }
            else
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var find = db.Deparments.Where(t => t.DepartmentNumber == Convert.ToInt32(TextBoxNumber.Text)).FirstOrDefault();
                    if (find != null)
                    {
                        if (insertDepartment != null)
                        {
                            if (insertDepartment.DepartmentId != find.DepartmentId)
                            {
                                hasMistake = true;
                                TextBoxNumberError.Content = "Данный номер подразделения уже существует";
                            }
                        }
                        else
                        {
                            hasMistake = true;
                            TextBoxNumberError.Content = "Данный номер подразделения уже существует";
                        }

                    }
                }
            }

            if (TextBoxFullName.Text == string.Empty)
            {
                hasMistake = true;
                TextBoxFullNameError.Content = "Обязательное поле";
            }

            if (TextBoxShortName.Text == string.Empty)
            {
                hasMistake = true;
                TextBoxShortNameError.Content = "Обязательное поле";
            }

            return !hasMistake;
        }

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
                    if (insertDepartment == null)
                    {
                        CreatyNewDepartment();
                    }
                    else
                    {
                        EditCurrentDepartment();
                    }
                    Close();
                }
                else
                {
                    Close();
                }
            }
        }

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
    }
}
