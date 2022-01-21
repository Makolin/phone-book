using Phone_Book.PagesDepartments;
using Phone_Book.PagesPosition;
using System.Windows.Controls;

namespace Phone_Book.Pages
{
    public partial class ReferencesPage : Page
    {
        public ReferencesPage()
        {
            InitializeComponent();
        }

        private void ReferenceMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Department.IsSelected)
            {
                ReferenceFrame.Content = new DepartmentPage();
            }
            if (Position.IsSelected)
            {
                ReferenceFrame.Content = new PositionPage();
            }
        }
    }
}
