using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            // Getting data from the user
            string clientName = ClientNameTextBox.Text;
            string clientPhoneNumber = PhoneNumberTextBox.Text;

            // Validating the entered data
            bool isValidName = ValidateInput(clientName, ClientNameTextBox, "Name is too short");
            bool isValidPhoneNumber = ValidateInput(clientPhoneNumber, PhoneNumberTextBox, "Phone number is too short");

            // If the data is valid, add the client
            if (isValidName && isValidPhoneNumber)
            {
                AddCustomer(clientName, clientPhoneNumber);
                ClearInputFields();
            }
        }

        private bool ValidateInput(string input, TextBox textBox, string errorMessage)
        {
            // Checking the length of the entered text
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
            // Adding a new client to the database
            DateTime currentDateTime = DateTime.Now;
            string currentDateTimeString = currentDateTime.ToString();

            Customer customer = new Customer(name, phoneNumber, currentDateTimeString, currentDateTimeString);
            bd.Customers.Add(customer);
            bd.SaveChanges();
        }

        private void ClearInputFields()
        {
            // Clearing input fields after successfully adding a client
            PhoneNumberTextBox.Text = "";
            ClientNameTextBox.Text = "";
        }
    }
}
