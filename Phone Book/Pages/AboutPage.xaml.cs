using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Phone_Book.Pages
{
    /// <summary>
    /// Логика взаимодействия для AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Window
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка переменных в окно
            Versions.TextWrapping = TextWrapping.Wrap;
            Version.Text = "Версия приложения - 0.02";
            MainText.Text = "Phone Book разработан @makolin и распространяется по MIT License. " +
                "Все пожелания по внесению изменений в приложение просьба отправлять на почтовый адрес makolin@bk.ru";

            ImageAbout.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/About.png"));

            // Загрузка описания версий приложения
            StreamReader streamReader = new StreamReader(Directory.GetCurrentDirectory() + @"\Resources\Versions.txt");
            string stringInsert = string.Empty;

            while (!streamReader.EndOfStream)
            {
                stringInsert += streamReader.ReadLine() + "\n";
            }
            streamReader.Close();
            Versions.Text = stringInsert;
        }
    }
}
