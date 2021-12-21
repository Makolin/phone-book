using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phone_Book.PagesTechnicalSupport
{
    public partial class TechnicalSupportPage : Page
    {
        public static ObservableCollection<TechnicalSupport> TechnicalSupports = new();
        private static void GetDataInDataBase()
        {
            using (ApplicationContext db = new())
            {
                TechnicalSupports = new ObservableCollection<TechnicalSupport>(db.TechnicalSupports.Include(t => t.UserSupport));
            }
        }
        public TechnicalSupportPage()
        {
            InitializeComponent();
            GetDataInDataBase();
            TechnicalSupportGrid.ItemsSource = TechnicalSupports;

            if (Authorization.authorization)
            {
                MainContextMenu.Visibility = Visibility.Visible;
            }
        }

        private void NewTechnicalSupport_Click(object sender, RoutedEventArgs e)
        {
            EditTechnicalSupportWindow editTechnicalSupportPage = new(null);
            editTechnicalSupportPage.ShowDialog();
        }

        private void EditTechnicalSupport_Click(object sender, RoutedEventArgs e)
        {
            TechnicalSupport editTechnicalSupport = (TechnicalSupport)TechnicalSupportGrid.SelectedItem;
            if (editTechnicalSupport != null)
            {
                EditTechnicalSupportWindow editTechnicalSupportPage = new(editTechnicalSupport);
                editTechnicalSupportPage.ShowDialog();
            }
        }

        private void DeleteTechnicalSupport_Click(object sender, RoutedEventArgs e)
        {
            TechnicalSupport deleteTechnicalSupport = (TechnicalSupport)TechnicalSupportGrid.SelectedItem;
            if (deleteTechnicalSupport != null)
            {
                MessageBoxResult result = MessageBox.Show(
                     $"Вы действительно хотите удалить пользователя с именем \"{deleteTechnicalSupport.UserSupport.Name}\" из базы данных?",
                     "Удалить",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    using (ApplicationContext db = new())
                    {
                        TechnicalSupports.Remove(deleteTechnicalSupport);
                        db.TechnicalSupports.Remove(deleteTechnicalSupport);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
