using EmployeeManagment.Models;
using System.Windows;

namespace EmployeeManagment
{
    public partial class App : Application
    {
        public static EmpManagmentEntities Context = new EmpManagmentEntities();
    }
}
