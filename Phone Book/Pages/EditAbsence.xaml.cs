using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Phone_Book.Pages
{
    public partial class EditAbsence : Window
    {
        public static ObservableCollection<Absence> AbcensesCollection = new ObservableCollection<Absence>();

        private static void GetData()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                AbcensesCollection = new ObservableCollection<Absence>(db.Absences.ToList());
            }
        }
        public EditAbsence(User currentUser)
        {
            InitializeComponent();
            GetData();
            DepartmentGrid.ItemsSource = AbcensesCollection;
            NameUser.Content = $"Список всех отсутствий пользователя - {currentUser.Name}";
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
            Close();
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
    }
}
