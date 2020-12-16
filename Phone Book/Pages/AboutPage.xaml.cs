using System;
using System.Collections.Generic;
using System.IO;
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
            MainText.Text = "Phone Book создан @makolin и распространяется по MIT License. " +
                "Все пожелания по внесению изменений в приложение просьба отправлять на почтовый адрес makolin@bk.ru";
        }
        private void Versions_Loaded(object sender, RoutedEventArgs e)
        {
            //Versions.IsReadOnly = true;
            Versions.TextWrapping = TextWrapping.Wrap;

            StreamReader streamReader = new StreamReader(Directory.GetCurrentDirectory() + @"\Resources\Versions.txt");
            string stringInsert = "";

            while (!streamReader.EndOfStream)
            {
                stringInsert += streamReader.ReadLine() + "\n";
            }
            streamReader.Close();
            Versions.Text = stringInsert;
        }
    }
}
