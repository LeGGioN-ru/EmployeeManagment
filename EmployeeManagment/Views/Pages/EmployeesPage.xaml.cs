using EmployeeManagment.Models;
using EmployeeManagment.Views.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeManagment.Views.Pages
{
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            EmployeesListView.Items.Clear();

            foreach (Employee employee in App.Context.Employee)
                EmployeesListView.Items.Add(new EmployeeRecord(employee));
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeeEditPage());
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesListView.SelectedItem is EmployeeRecord employeeRecord)
            {
                employeeRecord.RemoveEmployee();
                UpdateDataGrid();
            }
        }

        private void EmployeesListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (EmployeesListView.SelectedItem is EmployeeRecord employeeRecord)
                NavigationService.Navigate(new EmployeeEditPage(employeeRecord.Employee));
        }
    }
}
