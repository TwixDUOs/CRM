using System;
using System.Linq;
using System.Windows;

namespace Management_SYS
{
    public partial class CabinetWindow : Window
    {
        private ApplicationContext1 db = new ApplicationContext1();

        public CabinetWindow()
        {
            InitializeComponent();
            UpdateNavigationText();
        }

        // Update navigation text based on the number of planned calls
        private void UpdateNavigationText()
        {
            int plannedCallsToday = GetPlannedCallsCountForToday();
            if (plannedCallsToday > 0)
            {
                NavigationTextBlock.Text = $"Planned calls today: {plannedCallsToday}";
            }
            else
            {
                NavigationTextBlock.Text = "Select an action from the left menu to get started.";
            }
        }

        // Get the count of planned calls for today from the database
        private int GetPlannedCallsCountForToday()
        {
            DateTime today = DateTime.Today;
            var plannedCalls = db.Customers
                .AsEnumerable()
                .Where(c => DateTime.TryParse(c.NextContact, out DateTime nextContactDate) && nextContactDate.Date == today)
                .ToList();
            int count = plannedCalls.Count();

            return count;
        }

        private void HistoryLink_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.Show();
            this.Close();
        }

        private void PlannedCallsLink_Click(object sender, RoutedEventArgs e)
        {
            PlannedCallsWindow planed_Calls = new PlannedCallsWindow();
            planed_Calls.Show();
            this.Close();
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            AddClient_Window addClient_Window = new AddClient_Window();
            addClient_Window.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
