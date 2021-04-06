using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Phone_Book.Pages
{
    public partial class ListDepartment : Window
    {
        public static ObservableCollection<Department> Departments = new ObservableCollection<Department>();

        public void GetDataInDataBase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Departments = new ObservableCollection<Department>(db.Deparments.ToList());
            }
        }
        public ListDepartment()
        {
            InitializeComponent();
            GetDataInDataBase();
            DepartmentGrid.ItemsSource = Departments;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите сохранять изменения?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in DepartmentGrid.Items)
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите не сохранять изменения?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes) this.Close();
        }
    }
}
