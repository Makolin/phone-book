using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Phone_Book.PagesTechnicalSupport
{
    public partial class EditTechnicalSupportWindow : Window
    {
        private TechnicalSupport insertTechnicalSupport;
        public EditTechnicalSupportWindow(TechnicalSupport technicalSupport)
        {
            InitializeComponent();
            InsertDataInComboBox(technicalSupport);
        }
        private void InsertDataInComboBox(TechnicalSupport technicalSupport)
        {
            using (ApplicationContext db = new())
            {
                var UserList = db.Users.OrderBy(t => t.Name).ToList();
                ComboBoxUser.ItemsSource = UserList;
                ComboBoxUser.DisplayMemberPath = "Name";

                if (technicalSupport != null)
                {
                    insertTechnicalSupport = technicalSupport;
                    ComboBoxUser.SelectedIndex = UserList.FindIndex(t => t.UserId == technicalSupport.UserId);

                    // Заполнить checkbox
                    if (technicalSupport.SupportText.Contains("АСУП Eludia"))
                    {
                        CheckBoxEludia.IsChecked = true;
                    }

                    if (technicalSupport.SupportText.Contains("1C:Документооборот"))
                    {
                        CheckBoxDO.IsChecked = true;
                    }

                    if (technicalSupport.SupportText.Contains("1С:Управление производственным предприятием"))
                    {
                        CheckBoxUPP.IsChecked = true;
                    }

                    if (technicalSupport.SupportText.Contains("T-FLEX CAD"))
                    {
                        CheckBoxCAD.IsChecked = true;
                    }

                    if (technicalSupport.SupportText.Contains("T-FLEX DOCs"))
                    {
                        CheckBoxDOCs.IsChecked = true;
                    }

                    if (technicalSupport.SupportText.Contains("T-FLEX DOCs Технология"))
                    {
                        CheckBoxDocsTechnology.IsChecked = true;
                    }

                    if (technicalSupport.SupportText.Contains("СЭД Диадок"))
                    {
                        CheckBoxDiadoc.IsChecked = true;
                    }

                    if (technicalSupport.SupportText.Contains("Техническая поддержка пользователей"))
                    {
                        CheckBoxAllSupport.IsChecked = true;
                    }
                }
            }
        }

        private bool CheckTextInsert()
        {
            bool hasMistake = false;

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
                    if (insertTechnicalSupport == null)
                    {
                        CreatyNewTechnicalSupport();
                    }
                    else
                    {
                        EditCurrentTechnicalSupport();
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

        private void CreatyNewTechnicalSupport()
        {
            using (ApplicationContext db = new())
            {
                TechnicalSupport newTechnicalSupport = new();
                newTechnicalSupport.UserId = insertTechnicalSupport.UserId;

                // Заполняем техническую поддержку
                if (CheckBoxEludia.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "АСУП Eludia";
                }

                if (CheckBoxDO.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "\n1C:Документооборот";
                }

                if (CheckBoxUPP.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "\n1С:Управление производственным предприятием";
                }

                if (CheckBoxCAD.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "\nT-FLEX CAD";
                }

                if (CheckBoxDOCs.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "\nT-FLEX DOCs";
                }

                if (CheckBoxDocsTechnology.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "\nT-FLEX DOCs Технология";
                }

                if (CheckBoxDiadoc.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "\nСЭД Диадок";
                }

                if (CheckBoxAllSupport.IsChecked == true)
                {
                    newTechnicalSupport.SupportText = "\nТехническая поддержка пользователей";
                }

                TechnicalSupportPage.TechnicalSupports.Add(newTechnicalSupport);
                db.Entry(newTechnicalSupport).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        private void EditCurrentTechnicalSupport()
        {

        }
    }
}
