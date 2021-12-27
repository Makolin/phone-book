using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

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
            using (ApplicationContext db = new ApplicationContext())
            {
                var UserList = db.Users.OrderBy(t => t.Name).ToList();
                ComboBoxUser.ItemsSource = UserList;
                ComboBoxUser.DisplayMemberPath = "Name";

                if (technicalSupport != null)
                {
                    insertTechnicalSupport = technicalSupport;
                    ComboBoxUser.SelectedIndex = UserList.FindIndex(t => t.UserId == technicalSupport.UserId);
                    ComboBoxUser.IsEnabled = false;

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

        // Проверка на ошибки
        private bool CheckTextInsert()
        {
            bool hasMistake = false;
            ComboBoxUserError.Content = string.Empty;

            if (string.IsNullOrEmpty(ComboBoxUser.Text))
            {
                ComboBoxUserError.Content = "Обязательное поле";
                hasMistake = true;
            }

            else if (CheckBoxEludia.IsChecked == false && CheckBoxDO.IsChecked == false && CheckBoxDOCs.IsChecked == false && CheckBoxCAD.IsChecked == false
                && CheckBoxDocsTechnology.IsChecked == false && CheckBoxUPP.IsChecked == false && CheckBoxAllSupport.IsChecked == false)
            {
                MessageBox.Show("Не выбрано не одной технической поддержки");
                hasMistake = true;
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

        // Добавить проверку на уже существующего в ТП
        private void CreatyNewTechnicalSupport()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var find = db.Users.Where(t => t.Name == ComboBoxUser.Text).FirstOrDefault();

                if (find != null)
                {
                    string supportText = string.Empty;

                    TechnicalSupport newTechnicalSupport = new TechnicalSupport()
                    {
                        UserId = find.UserId
                    };

                    // Заполняем техническую поддержку
                    if (CheckBoxEludia.IsChecked == true)
                    {
                        supportText = "АСУП Eludia";
                    }

                    if (CheckBoxDO.IsChecked == true)
                    {
                        if (supportText != string.Empty)
                        {
                            supportText += "\n";
                        }
                        supportText += "1C:Документооборот";
                    }

                    if (CheckBoxUPP.IsChecked == true)
                    {
                        if (supportText != string.Empty)
                        {
                            supportText += "\n";
                        }
                        supportText += "1С:Управление производственным предприятием";
                    }

                    if (CheckBoxCAD.IsChecked == true)
                    {
                        if (supportText != string.Empty)
                        {
                            supportText += "\n";
                        }
                        supportText += "T-FLEX CAD";
                    }

                    if (CheckBoxDOCs.IsChecked == true)
                    {
                        if (supportText != string.Empty)
                        {
                            supportText += "\n";
                        }
                        supportText += "T-FLEX DOCs";
                    }

                    if (CheckBoxDocsTechnology.IsChecked == true)
                    {
                        if (supportText != string.Empty)
                        {
                            supportText += "\n";
                        }
                        supportText += "T-FLEX DOCs Технология";
                    }

                    if (CheckBoxDiadoc.IsChecked == true)
                    {
                        if (supportText != string.Empty)
                        {
                            supportText += "\n";
                        }
                        supportText += "СЭД Диадок";
                    }

                    if (CheckBoxAllSupport.IsChecked == true)
                    {
                        if (supportText != string.Empty)
                        {
                            supportText += "\n";
                        }
                        supportText += "Техническая поддержка пользователей";
                    }

                    if (supportText != string.Empty)
                    {
                        newTechnicalSupport.SupportText = supportText;
                    }

                    TechnicalSupportPage.TechnicalSupports.Add(newTechnicalSupport);
                    db.Entry(newTechnicalSupport).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
        }

        private void EditCurrentTechnicalSupport()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (ComboBoxUser.SelectedIndex != -1)
                {
                    var find = db.Users.Where(t => t.Name == ComboBoxUser.Text).FirstOrDefault();

                    if (find != null)
                    {
                        string supportText = string.Empty;

                        // Заполняем техническую поддержку c нуля
                        if (CheckBoxEludia.IsChecked == true)
                        {
                            supportText = "АСУП Eludia";
                        }

                        if (CheckBoxDO.IsChecked == true)
                        {
                            if (supportText != string.Empty)
                            {
                                supportText += "\n";
                            }
                            supportText += "1C:Документооборот";
                        }

                        if (CheckBoxUPP.IsChecked == true)
                        {
                            if (supportText != string.Empty)
                            {
                                supportText += "\n";
                            }
                            supportText += "1С:Управление производственным предприятием";
                        }

                        if (CheckBoxCAD.IsChecked == true)
                        {
                            if (supportText != string.Empty)
                            {
                                supportText += "\n";
                            }
                            supportText += "T-FLEX CAD";
                        }

                        if (CheckBoxDOCs.IsChecked == true)
                        {
                            if (supportText != string.Empty)
                            {
                                supportText += "\n";
                            }
                            supportText += "T-FLEX DOCs";
                        }

                        if (CheckBoxDocsTechnology.IsChecked == true)
                        {
                            if (supportText != string.Empty)
                            {
                                supportText += "\n";
                            }
                            supportText += "T-FLEX DOCs Технология";
                        }

                        if (CheckBoxDiadoc.IsChecked == true)
                        {
                            if (supportText != string.Empty)
                            {
                                supportText += "\n";
                            }
                            supportText += "СЭД Диадок";
                        }

                        if (CheckBoxAllSupport.IsChecked == true)
                        {
                            if (supportText != string.Empty)
                            {
                                supportText += "\n";
                            }
                            supportText += "Техническая поддержка пользователей";
                        }

                        if (supportText != string.Empty)
                        {
                            insertTechnicalSupport.SupportText = supportText;
                        }

                        TechnicalSupportPage.TechnicalSupports.Remove(insertTechnicalSupport);
                        db.Entry(insertTechnicalSupport).State = EntityState.Modified;
                        db.SaveChanges();
                        TechnicalSupportPage.TechnicalSupports.Add(insertTechnicalSupport);
                    }
                }
            }
        }
    }
}
