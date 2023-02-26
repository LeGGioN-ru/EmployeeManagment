using EmployeeManagment.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;

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
            PrintButton.Visibility = Visibility.Collapsed;
        }

        public EmployeeEditPage(Employee employee)
        {
            InitializeComponent();

            _employee = employee;
            SetButtonName(employee);
            BindingData(employee);
        }

        private void BindingData(Employee employee)
        {
            FullNameTextBox.Text = employee.Employee_surname + " " + employee.Employee_name + " " + employee.Employee_patronymic;
            DateBirthPicker.SelectedDate = employee.Birthday;
            PhoneNumberTextBox.Text = employee.Phone_number_emp;
            AddressTextBox.Text = employee.Address;
            PassportDataTextBox.Text = employee.Passport_data.Passport_serial + " " + employee.Passport_data.Passport_number;
            DateIssuePicker.SelectedDate = employee.Emp_discription.Date_of_employment;
            EmployeeTypeComboBox.SelectedItem = employee.Emp_discription.Emp_type;
            PostComboBox.SelectedItem = employee.Post;
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
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
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
            _employee.Post = PostComboBox.SelectedItem as Post;
            _employee.Emp_discription.Emp_type = EmployeeTypeComboBox.SelectedItem as Emp_type;
            _employee.Emp_discription.Date_of_employment = DateIssuePicker.SelectedDate.Value;
            _employee.Address = AddressTextBox.Text;
            _employee.Birthday = DateBirthPicker.SelectedDate.Value;
            _employee.Employee_name = FullNameTextBox.Text.Split(' ')[1];
            _employee.Employee_surname = FullNameTextBox.Text.Split()[0];
            _employee.Employee_patronymic = FullNameTextBox.Text.Split(' ').Length == 3 ? FullNameTextBox.Text.Split(' ')[2] : string.Empty;
            _employee.Phone_number_emp = PhoneNumberTextBox.Text;
        }

        private void SetButtonName(Employee employee = null)
        {
            if (employee == null)
                AddEditTextBlock.Text = "Добавить";
            else
                AddEditTextBlock.Text = "Изменить";
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            EmployeeTypeComboBox.ItemsSource = App.Context.Emp_type.ToList();
            PostComboBox.ItemsSource = App.Context.Post.ToList();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Word.Application wordApp = new Word.Application();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Word Document (*.docx)|*.docx";
            saveDialog.Title = "Save Word Document";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                Word.Document wordDoc = wordApp.Documents.Add();

                wordDoc.Content.Text = _employee.ToString();

                wordDoc.SaveAs2(saveDialog.FileName);

                wordDoc.Close();
                wordApp.Quit();
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesPage());
        }
    }
}
