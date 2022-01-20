using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Phone_Book.PagesPosition
{
    public partial class PositionPage : Page
    {
        public static ObservableCollection<Position> Positions = new ObservableCollection<Position>();
        private static void GetDataInDataBase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Positions = new ObservableCollection<Position>(db.Positions.ToList());
            }
        }
        public PositionPage()
        {
            InitializeComponent();
            GetDataInDataBase();
            PositionGrid.ItemsSource = Positions;

            if (Authorization.authorization)
            {
                MainContextMenu.Visibility = Visibility.Visible;
            }
        }

        private void NewPositon_Click(object sender, RoutedEventArgs e)
        {
            EditPositionWindow newPositionWindows = new EditPositionWindow(null);
            newPositionWindows.ShowDialog();
        }

        private void EditPosition_Click(object sender, RoutedEventArgs e)
        {
            var editPosition = (Position)PositionGrid.SelectedItem;
            if (editPosition != null)
            {
                EditPositionWindow editUserPage = new EditPositionWindow(editPosition);
                editUserPage.ShowDialog();
            }
        }

        private void DeletePosition_Click(object sender, RoutedEventArgs e)
        {
            Position deletePosition = (Position)PositionGrid.SelectedItem;
            if (deletePosition != null)
            {
                // Проверка на наличие пользователей с данной должностью
                using (ApplicationContext db = new ApplicationContext())
                {
                    int countPosition = default;
                    var find = db.Users.Where(t => t.PositionId == deletePosition.PositionId);
                    if (find != null)
                    {
                        countPosition = find.Count();
                    }

                    if (countPosition == 0)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"Вы действительно хотите удалить пользователя с именем \"{deletePosition.PositionName}\" из базы данных?",
                            "Удалить",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            Positions.Remove(deletePosition);
                            db.Positions.Remove(deletePosition);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Нельзя удалить \"{deletePosition.PositionName}\", так как имеются пользователи с данной должностью.");
                    }
                }
            }
        }
    }
}
