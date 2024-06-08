using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Management_SYS
{
    public partial class PlannedCallsWindow : Window
    {
        private ApplicationContext1 dbContext;
        private List<Customer> ListOfCustomers;

        public PlannedCallsWindow()
        {
            InitializeComponent();
            dbContext = new ApplicationContext1();
            LoadPlannedCalls();
            LoadCustomers();
            PlannedCallsDataGrid.SelectionChanged += PlannedCallsDataGrid_SelectionChanged;
        }

        // Load planned calls for today
        private void LoadPlannedCalls()
        {
            DateTime today = DateTime.Today;
            var plannedCalls = dbContext.Customers
                .ToList()
                .Where(c => DateTime.TryParse(c.NextContact, out DateTime nextContactDate) && nextContactDate.Date == today)
                .Select(c => new PlannedCallViewModel
                {
                    NameOfCustomer = c.NameOfCustomer,
                    PhoneNumberOfCustomer = c.PhoneNumberOfCustomer,
                    LastContact = Convert.ToDateTime(c.LastContact),
                    NextContact = Convert.ToDateTime(c.NextContact),
                    CustomerID = c.customerID,
                    LastComment = GetLastComment(c.customerID)
                })
                .ToList();

            PlannedCallsDataGrid.ItemsSource = plannedCalls;
        }

        // Load all customers
        private void LoadCustomers()
        {
            var customers = dbContext.Customers.ToList();
            if (customers != null)
            {
                ListOfCustomers = customers;
            }
        }

        // Get the last comment for a given customer
        private string GetLastComment(int customerId)
        {
            var lastContact = dbContext.Contacts_story
                .Where(contact => contact.Id_of_customer == customerId)
                .OrderByDescending(contact => contact.Time_of_contact)
                .FirstOrDefault();

            return lastContact != null ? lastContact.Contact_info : string.Empty;
        }

        // Navigate back to the cabinet window
        private void BackToCabinet_Click(object sender, RoutedEventArgs e)
        {
            var cabinetWindow = new CabinetWindow();
            cabinetWindow.Show();
            Close();
        }

        // Handle selection change in the DataGrid
        private void PlannedCallsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPlannedCall = PlannedCallsDataGrid.SelectedItem as PlannedCallViewModel;

            if (selectedPlannedCall != null)
            {
                int customerId = selectedPlannedCall.CustomerID;
                string lastComment = GetLastComment(customerId);

                // Open the CardWindow and pass the selected customer and database context
                Customer selectedCustomer = dbContext.Customers.FirstOrDefault(c => c.customerID == customerId);
                CardWindow cardWindow = new CardWindow(selectedCustomer, dbContext);
                cardWindow.Owner = this; // Set the owner of the CardWindow
                cardWindow.ShowDialog(); // Show the window as a dialog
            }
        }
    }

    // ViewModel class for planned calls
    public class PlannedCallViewModel
    {
        public string NameOfCustomer { get; set; }
        public string PhoneNumberOfCustomer { get; set; }
        public DateTime LastContact { get; set; }
        public DateTime NextContact { get; set; }
        public int CustomerID { get; set; }
        public string LastComment { get; set; }
    }
}
