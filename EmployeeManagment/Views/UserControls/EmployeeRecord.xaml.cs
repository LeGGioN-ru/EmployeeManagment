using EmployeeManagment.Models;
using System.Windows.Controls;

namespace EmployeeManagment.Views.UserControls
{
    public partial class EmployeeRecord : UserControl
    {
        private Employee _employee;

        public EmployeeRecord(Employee employee)
        {
            InitializeComponent();
            DataContext = employee;
            _employee = employee;
        }

        public void RemoveEmployee()
        {
            App.Context.Employee.Remove(_employee);
            App.Context.SaveChanges();
        }
    }
}
