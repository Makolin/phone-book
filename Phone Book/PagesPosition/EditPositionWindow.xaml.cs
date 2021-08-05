using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Phone_Book.PagesPosition
{
    public partial class EditPositionWindow : Window
    {
        Position insertPosition;
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
                Position newPosition = new Position();
                newPosition.PositionName = TextBoxName.Text;

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

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите сохранять изменения?",
                "Сохранение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (insertPosition == null) CreatyNewPosition();
                else EditCurrentPosition();
                this.Close();
            }
            else
            {
                this.Close();
            }
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

        private void ValidatonOnlyText(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
