using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

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
            // Получение истории контактов для выбранного клиента
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

            // Отображение истории контактов в ListView
            ContactHistoryListView.ItemsSource = contactHistory;
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение информации о новом контакте из элементов управления
            string contactInfo = ContactInfoTextBox.Text;

            // Создание нового объекта контакта
            Contact_story newContact = new Contact_story()
            {
                Id_of_customer = selectedCustomer.customerID,
                Time_of_contact = DateTime.Now.ToString(),
                Contact_info = contactInfo
            };

            // Обновление времени последнего контакта в объекте Customer
            selectedCustomer.LastContact = DateTime.Now.ToString();
            DateTime? nextContactOitput = NextContactDatePicker.Value;
            selectedCustomer.NextContact = nextContactOitput.ToString();

            // Добавление нового контакта в базу данных
            dbContext.Contacts_story.Add(newContact);
            dbContext.SaveChanges();

            // Обновление времени последнего контакта в таблице Customers
            // Находим объект Customer в контексте базы данных по его ID
            Customer customerToUpdate = dbContext.Customers.Find(selectedCustomer.customerID);

            // Проверяем, найден ли объект Customer
            if (customerToUpdate != null)
            {
                // Обновляем значение last contact
                customerToUpdate.LastContact = DateTime.Now.ToString();

                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();
            }

            // Обновление списка истории контактов
            LoadContactHistory();

            ContactInfoTextBox.Text = "";
            NextContactDatePicker.Text = "";
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}