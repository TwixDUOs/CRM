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
            UpdateNavigationText(); // Update navigation text on window initialization
        }

        // Update navigation text based on the number of planned calls
        private void UpdateNavigationText()
        {
            // Перевірка на помилки підключення, щоб вікно не падало при старті
            try
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
            catch
            {
                NavigationTextBlock.Text = "Database connection error or no data.";
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

        // Handle click event for history link
        private void HistoryLink_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.Show();
            this.Close();
        }

        // Handle click event for planned calls link
        private void PlannedCallsLink_Click(object sender, RoutedEventArgs e)
        {
            PlannedCallsWindow plannedCallsWindow = new PlannedCallsWindow();
            plannedCallsWindow.Show();
            this.Close();
        }

        // Handle click event for adding a new client
        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            AddClient_Window addClientWindow = new AddClient_Window();
            addClientWindow.Show();
            this.Close();
        }

        // --- НОВИЙ МЕТОД ДЛЯ ВІДКРИТТЯ АНАЛІТИКИ ---
        private void Analytics_Click(object sender, RoutedEventArgs e)
        {
            // Створюємо екземпляр вікна аналітики (з попереднього кроку)
            AnalyticsWindow analyticsWindow = new AnalyticsWindow();
            analyticsWindow.Show();

            // Якщо хочеш, щоб головне меню закривалось - залиш цей рядок. 
            // Якщо ні - закоментуй this.Close();
            // this.Close(); 
        }

        // Handle click event for the exit button
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}