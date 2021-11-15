using Phone_Book.PagesDepartments;
using Phone_Book.PagesPosition;
using Phone_Book.PagesTechnicalSupport;
using System.Windows;

namespace Phone_Book.Pages
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Content = new MainPage();
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new MainPage();
        }

        private void DepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new DepartmentPage();
        }

        private void PositionButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new PositionPage();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            AboutPage aboutPage = new AboutPage();
            aboutPage.ShowDialog();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TechnicalSupportButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new TechnicalSupportPage();
        }
    }
}
