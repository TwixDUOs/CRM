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


namespace Management_SYS
{
    public partial class AddClient_Window : Window
    {
        ApplicationContext1 bd;
        public AddClient_Window()
        {
            InitializeComponent();

            bd = new ApplicationContext1();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.Show();
            this.Close();

        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримання даних від користувача
            string clientName = ClientNameTextBox.Text;
            string clientPhoneNumber = PhoneNumberTextBox.Text;

            // Перевірка на коректність введених даних
            bool isValidName = ValidateInput(clientName, ClientNameTextBox, "Name is too short");
            bool isValidPhoneNumber = ValidateInput(clientPhoneNumber, PhoneNumberTextBox, "Phone number is too short");

            // Якщо дані введені коректно, то додаємо клієнта
            if (isValidName && isValidPhoneNumber)
            {
                AddCustomer(clientName, clientPhoneNumber);
                ClearInputFields();
            }
        }

        private bool ValidateInput(string input, TextBox textBox, string errorMessage)
        {
            // Перевірка на довжину введеного тексту
            if (input.Length <= 1)
            {
                textBox.ToolTip = errorMessage;
                textBox.Background = Brushes.Red;
                return false;
            }
            else
            {
                textBox.ToolTip = "";
                textBox.Background = Brushes.Transparent;
                return true;
            }
        }

        private void AddCustomer(string name, string phoneNumber)
        {
            // Додавання нового клієнта до бази даних
            DateTime currentDateTime = DateTime.Now;
            string currentDateTimeString = currentDateTime.ToString();

            Customer customer = new Customer(name, phoneNumber, currentDateTimeString, currentDateTimeString);
            bd.Customers.Add(customer);
            bd.SaveChanges();
        }

        private void ClearInputFields()
        {
            // Очищення полів вводу після успішного додавання клієнта
            PhoneNumberTextBox.Text = "";
            ClientNameTextBox.Text = "";
        }
    }
}
