using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Phone_Book.PagesPosition
{
    public partial class EditPositionWindow : Window
    {
        private Position insertPosition;

        public EditPositionWindow(Position currentPosition)
        {
            InitializeComponent();
            InsertDataInComboBox(currentPosition);
        }

        private void InsertDataInComboBox(Position position)
        {
            if (position != null)
            {
                insertPosition = position;
                TextBoxName.Text = position.PositionName;
            }
        }

        // Создание новой должности
        private void CreatyNewPosition()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Position newPosition = new Position
                {
                    PositionName = TextBoxName.Text
                };

                PositionPage.Positions.Add(newPosition);
                db.Entry(newPosition).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        // Редактирование выбранной должности
        private void EditCurrentPosition()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                PositionPage.Positions.Remove(insertPosition);
                insertPosition.PositionName = TextBoxName.Text;
                db.Entry(insertPosition).State = EntityState.Modified;
                db.SaveChanges();
                PositionPage.Positions.Add(insertPosition);
            }
        }

        // Проверка на заполнение обязательного поля
        // Проверка на уникальность наименования должности
        private bool CheckTextInsert()
        {
            bool hasMistake = false;
            TextBoxNameError.Content = string.Empty;

            if (TextBoxName.Text == string.Empty)
            {
                hasMistake = true;
                TextBoxNameError.Content = "Обязательное поле";
            }
            else
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var find = db.Positions.Where(t => t.PositionName == TextBoxName.Text).FirstOrDefault();
                    if (find != null)
                    {
                        if (insertPosition != null)
                        {
                            if (insertPosition.PositionId != find.PositionId)
                            {
                                hasMistake = true;
                                TextBoxNameError.Content = "Данная должность уже существует";
                            }
                        }
                        else
                        {
                            hasMistake = true;
                            TextBoxNameError.Content = "Данная должность уже существует";
                        }
                    }
                }
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
                    if (insertPosition == null)
                    {
                        CreatyNewPosition();
                    }
                    else
                    {
                        EditCurrentPosition();
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

        private void ValidatonOnlyText(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
