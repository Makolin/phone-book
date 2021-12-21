using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Phone_Book.PagesDepartments
{
    public partial class DepartmentPage : Page
    {
        public static ObservableCollection<Department> Departments = new ObservableCollection<Department>();
        private static void GetDataInDataBase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Departments = new ObservableCollection<Department>(db.Deparments);
            }
        }
        public DepartmentPage()
        {
            InitializeComponent();
            GetDataInDataBase();
            DepartmentGrid.ItemsSource = Departments;

            if (Authorization.authorization)
            {
                MainContextMenu.Visibility = Visibility.Visible;
            }
        }

        private void NewDepartment_Click(object sender, RoutedEventArgs e)
        {
            EditDepartmentWindow newDepartmentWindows = new EditDepartmentWindow(null);
            newDepartmentWindows.ShowDialog();
        }

        private void EditDepartment_Click(object sender, RoutedEventArgs e)
        {
            var editDepartment = (Department)DepartmentGrid.SelectedItem;
            if (editDepartment != null)
            {
                EditDepartmentWindow editUserPage = new EditDepartmentWindow(editDepartment);
                editUserPage.ShowDialog();
            }
        }

        private void DeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            var deleteDepartment = (Department)DepartmentGrid.SelectedItem;
            if (deleteDepartment != null)
            {
                // Проверка на наличие пользователей с данной должностью
                using (ApplicationContext db = new ApplicationContext())
                {
                    int countDepartment = 0;
                    var find = db.Users.Where(t => t.DepartmentId == deleteDepartment.DepartmentId);
                    if (find != null)
                    {
                        countDepartment = find.Count();
                    }

                    if (countDepartment == 0)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы действительно хотите удалить пользователя с именем \"{deleteDepartment.DepartmentFullName}\" из базы данных?",
                            "Удалить",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            Departments.Remove(deleteDepartment);
                            db.Deparments.Remove(deleteDepartment);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Нельзя удалить \"{deleteDepartment.DepartmentFullName}\", так как имеются пользователи в данном подразделении.");
                    }
                }
            }
        }
    }
}
