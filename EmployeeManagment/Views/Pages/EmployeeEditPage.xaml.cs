using EmployeeManagment.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeManagment.Views.Pages
{
    public partial class EmployeeEditPage : Page
    {
        private Employee _employee;

        private bool _isEmployeeNull => _employee == null;

        public EmployeeEditPage()
        {
            InitializeComponent();

            SetButtonName();
        }

        public EmployeeEditPage(Employee employee)
        {
            InitializeComponent();

            _employee = employee;
            SetButtonName(employee);
        }

        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] fullName = FullNameTextBox.Text.Split(' ');
                string[] passportData = PassportDataTextBox.Text.Split(' ');

                ValidateFullName(fullName);
                ValidatePassportData(passportData);
                ValidateDates(DateIssuePicker.SelectedDate.Value, DateTime.Now);
                ValidatePhoneNumber(PhoneNumberTextBox.Text);

                if (EmployeeTypeComboBox.SelectedItem == null)
                    throw new ArgumentException("Тип занятости не выбран");

                if (PostComboBox.SelectedItem == null)
                    throw new ArgumentException("Позиция работника не выбрана");

                if (_isEmployeeNull)
                    Add();
                else
                    Edit();

                App.Context.SaveChanges();
                NavigationService.Navigate(new EmployeesPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ValidateFullName(string[] fullName)
        {
            if (fullName.Length < 2 || fullName.Length > 3)
                throw new ArgumentException("ФИО введено не корректно");

            foreach (string fullNamePart in fullName)
                if (fullNamePart.Any(ch => char.IsLetter(ch) == false))
                    throw new ArgumentException("В имени присутствуют не только буквы");
        }

        private void ValidatePassportData(string[] passportData)
        {
            if (passportData.Length != 2)
                throw new ArgumentException("Пасспортные данные введены не корректны");

            string serial = passportData[0];
            string number = passportData[1];

            if (serial.Length != 4)
                throw new ArgumentException("Серия введена не правильно");

            if (number.Length != 6)
                throw new ArgumentException("Номер введён не правильно");

            if (App.Context.Employee.Count(x => x.Passport_data.Passport_serial == serial && x.Passport_data.Passport_number == number) > 1)
                throw new ArgumentException("Человек с такими пасспортными данными уже существует в базе данных");

            foreach (string passportDataPart in passportData)
                if (passportDataPart.Any(ch => char.IsDigit(ch) == false))
                    throw new ArgumentException("В серии и номере присутствуют не только цифры");
        }

        private void ValidateDates(DateTime dateIssue, DateTime dateBirth)
        {
            if (dateIssue.Date > DateTime.Now)
                throw new ArgumentException("Дата принятия не может быть больше чем сегодня");

            //if (dateBirth.Date.AddYears(18) > DateTime.Now)
            //    throw new ArgumentException("Работнику нету 18");
        }

        private void ValidatePhoneNumber(string phoneNumber)
        {
            string errorMessage = "Введите телефон в формате *(***)***-**-**, где * - это цифра";

            char openBreacked = '(';
            char closeBreacked = ')';
            char defice = '-';

            if (phoneNumber.Length != 15)
                throw new ArgumentException();

            if (phoneNumber[1] != openBreacked || phoneNumber[5] != closeBreacked || phoneNumber[9] != defice || phoneNumber[12] != defice)
                throw new ArgumentException(errorMessage);

            for (int i = 0; i < phoneNumber.Length; i++)
            {
                if (i == 1 || i == 5 || i == 9 || i == 12)
                    continue;

                if (char.IsDigit(phoneNumber[i]) == false)
                    throw new ArgumentException(errorMessage);
            }
        }

        private void Add()
        {
            Employee employee = new Employee()
            {
                Address = AddressTextBox.Text,
                Birthday = DateBirthPicker.SelectedDate.Value,
                Employee_name = FullNameTextBox.Text.Split(' ')[1],
                Employee_surname = FullNameTextBox.Text.Split()[0],
                Employee_patronymic = FullNameTextBox.Text.Split(' ').Length == 3 ? FullNameTextBox.Text.Split(' ')[2] : string.Empty,
                Phone_number_emp = PhoneNumberTextBox.Text,
                Post_id = (PostComboBox.SelectedItem as Post).Post_id,
            };

            App.Context.Employee.Add(employee);
            App.Context.SaveChanges();

            Passport_data passport_Data = new Passport_data()
            {
                Passport_number = PassportDataTextBox.Text.Split(' ')[1],
                Passport_serial = PassportDataTextBox.Text.Split(' ')[0],
                Employee = employee
            };

            App.Context.Passport_data.Add(passport_Data);

            Emp_discription emp_Discription = new Emp_discription()
            {
                Date_of_employment = DateIssuePicker.SelectedDate.Value,
                Emp_type_id = (EmployeeTypeComboBox.SelectedItem as Emp_type).Emp_type_id,
                Employee = employee
            };
            App.Context.Emp_discription.Add(emp_Discription);

            App.Context.SaveChanges();
        }

        private void Edit()
        {

        }

        private void SetButtonName(Employee employee = null)
        {
            if (employee == null)
                AddEditButton.Content = "Добавить";
            else
                AddEditButton.Content = "Изменить";
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            EmployeeTypeComboBox.ItemsSource = App.Context.Emp_type.ToList();
            PostComboBox.ItemsSource = App.Context.Post.ToList();
        }
    }
}
