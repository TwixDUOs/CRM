using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Management_SYS
{
    public partial class CardWindow : Window
    {
        private Customer selectedCustomer;
        private ApplicationContext1 dbContext;

        public CardWindow(Customer selectedCustomer, ApplicationContext1 dbContext)
        {
            InitializeComponent();
            this.selectedCustomer = selectedCustomer;
            this.dbContext = dbContext;
            LoadContactHistory();
        }

        private void LoadContactHistory()
        {
            // Getting the contact history for the selected customer
            var contactHistory = dbContext.Contacts_story
                .Where(c => c.Id_of_customer == selectedCustomer.customerID)
                .Select(contact => new
                {
                    selectedCustomer.NameOfCustomer,
                    selectedCustomer.PhoneNumberOfCustomer,
                    contact.Contact_info,
                    contact.Time_of_contact,
                    selectedCustomer.NextContact,
                    contact.id_of_contact
                })
                .ToList();

            // Displaying the contact history in ListView
            ContactHistoryListView.ItemsSource = contactHistory;
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            // Getting information about the new contact from the controls
            string contactInfo = ContactInfoTextBox.Text;
            DateTime? nextContactDate = NextContactDatePicker.Value;

            // Check if both contact information and next contact date are provided
            if (!string.IsNullOrWhiteSpace(contactInfo) && nextContactDate != null)
            {
                // Creating a new contact object
                Contact_story newContact = new Contact_story()
                {
                    Id_of_customer = selectedCustomer.customerID,
                    Time_of_contact = DateTime.Now.ToString(),
                    Contact_info = contactInfo
                };

                // Updating the last contact time in the Customer object
                selectedCustomer.LastContact = DateTime.Now.ToString();
                selectedCustomer.NextContact = nextContactDate.Value.ToString(); // Store the date directly without conversion

                // Adding the new contact to the database
                dbContext.Contacts_story.Add(newContact);
                dbContext.SaveChanges();

                // Finding the Customer object in the database context by its ID
                Customer customerToUpdate = dbContext.Customers.Find(selectedCustomer.customerID);

                // Checking if the Customer object is found
                if (customerToUpdate != null)
                {
                    // Updating the last contact value
                    customerToUpdate.LastContact = DateTime.Now.ToString();

                    // Saving changes to the database
                    dbContext.SaveChanges();
                }

                // Updating the contact history list
                LoadContactHistory();

                // Clear input fields
                ContactInfoTextBox.Text = "";
                NextContactDatePicker.Value = null;
            }
            else
            {
                MessageBox.Show("Please provide both contact information and next contact date.");
            }
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо ідентифікатор контакту, прив'язаний до кнопки
            var button = sender as Button;
            var contactToDelete = button.CommandParameter as dynamic;

            if (contactToDelete != null)
            {
                // Отримуємо ідентифікатор контакту
                int contactId = contactToDelete.id_of_contact;

                // Шукаємо контакт у базі даних за його ідентифікатором
                var contact = dbContext.Contacts_story.FirstOrDefault(c => c.id_of_contact == contactId);

                if (contact != null)
                {
                    // Видаляємо контакт з бази даних
                    dbContext.Contacts_story.Remove(contact);
                    dbContext.SaveChanges();

                    // Оновлюємо список після видалення
                    LoadContactHistory();
                }
                else
                {
                    MessageBox.Show("Не вдалося знайти контакт для видалення.");
                }
            }
            else
            {
                MessageBox.Show("Не вдалося отримати контакт для видалення.");
            }
        }

    }

    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double listViewWidth && parameter is string param)
            {
                //size of columns
                double phoneWidthPercent = 0.17;
                double timeWidthPercent = 0.15;
                double actionWidthPercent = 0.15;
                double nameWidthPercent = 0.18;
                double contactInfoWidthPercent = 0.39;

                switch (param)
                {
                    case "NameOfCustomer":
                        return listViewWidth * nameWidthPercent;
                    case "PhoneNumberOfCustomer":
                        return listViewWidth * phoneWidthPercent;
                    case "Time_of_contact":
                        return listViewWidth * timeWidthPercent;
                    case "Action":
                        return listViewWidth * actionWidthPercent;
                    case "Contact_info":
                        return listViewWidth * contactInfoWidthPercent;
                    default:
                        return listViewWidth / 5; // If something wrong all will get 20%
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
