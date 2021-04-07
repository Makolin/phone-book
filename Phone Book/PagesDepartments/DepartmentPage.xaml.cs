using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Phone_Book.Pages
{
    public partial class DepartmentPage : Page
    {

        public static ObservableCollection<Department> Departments = new ObservableCollection<Department>();
        public void GetDataInDataBase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Departments = new ObservableCollection<Department>(db.Deparments.ToList());
            }
        }
        public DepartmentPage()
        {
            InitializeComponent();
            GetDataInDataBase();
            DepartmentGrid.ItemsSource = Departments;
        }
    }
}
