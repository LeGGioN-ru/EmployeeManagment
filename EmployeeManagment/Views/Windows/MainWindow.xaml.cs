using EmployeeManagment.Views.Pages;
using System.Windows;

namespace EmployeeManagment
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            MainFrame.Navigate(new AuthorizationPage());
        }
    }
}
