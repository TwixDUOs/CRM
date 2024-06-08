using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Management_SYS
{
    public partial class HistoryWindow : Window
    {
        private ApplicationContext1 dbContext;

        public HistoryWindow()
        {
            InitializeComponent();
            dbContext = new ApplicationContext1();
            LoadContactHistory();
        }

        // Load contact history from the database
        private void LoadContactHistory()
        {
            try
            {
                var contacts = dbContext.Contacts_story
                    .Join(dbContext.Customers,
                        contact => contact.Id_of_customer,
                        customer => customer.customerID,
                        (contact, customer) => new ContactJoin
                        {
                            id_of_contact = contact.id_of_contact,
                            Time_of_contact = contact.Time_of_contact,
                            Contact_info = contact.Contact_info,
                            NameOfCustomer = customer.NameOfCustomer,
                            PhoneNumberOfCustomer = customer.PhoneNumberOfCustomer,
                            Id_of_customer = contact.Id_of_customer
                        })
                    .OrderByDescending(contact => contact.Time_of_contact)
                    .ToList();

                ListOfContacts.ItemsSource = contacts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data from the database: " + ex.Message);
            }
        }

        // Handle selection change in the contacts list
        private void ListOfCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListOfContacts.SelectedItem != null)
            {
                dynamic selectedContact = ListOfContacts.SelectedItem;
                int customerId = selectedContact.Id_of_customer;
                Customer selectedCustomer = dbContext.Customers.FirstOrDefault(c => c.customerID == customerId);
                CardWindow cardWindow = new CardWindow(selectedCustomer, dbContext);
                cardWindow.Owner = this;
                cardWindow.ShowDialog();
            }
        }

        // Class for joining contact and customer data
        public class ContactJoin
        {
            public int id_of_contact { get; set; }
            public string Time_of_contact { get; set; }
            public string Contact_info { get; set; }
            public string NameOfCustomer { get; set; }
            public string PhoneNumberOfCustomer { get; set; }
            public int Id_of_customer { get; set; }
        }

        // Handle exit button click
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.Show();
            this.Close();
        }
    }
}
