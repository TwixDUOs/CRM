using System;
using System.Linq;
using System.Windows;

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
                    selectedCustomer.NextContact
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

                // Updating the last contact time in the Customers table
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
    }
}
