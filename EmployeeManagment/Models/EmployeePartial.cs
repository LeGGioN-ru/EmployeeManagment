namespace EmployeeManagment.Models
{
    public partial class Employee
    {
        public override string ToString()
        {
            string finalString = string.Empty;

            finalString += $"ФИО: {Employee_surname} {Employee_name} {Employee_patronymic}\n";
            finalString += $"Телефон: {Phone_number_emp}\n";
            finalString += $"Дата рождения: {Birthday.ToShortDateString()}\n";
            finalString += $"Адрес: {Address}\n";
            finalString += $"Паспортные данные: {Passport_data.Passport_serial} {Passport_data.Passport_number}\n";
            finalString += $"Дата принятия на работу: {Emp_discription.Date_of_employment.ToShortDateString()}\n";
            finalString += $"Должность: {Post.Post_name} Зарплата: {Post.Salary.Salary_volume}₽\n";

            return finalString;
        }
    }
}
