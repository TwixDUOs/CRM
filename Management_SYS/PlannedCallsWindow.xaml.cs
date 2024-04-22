using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Management_SYS
{
    public partial class Planed_calls : Window
    {
        public ApplicationContext1 dbContext;

        public Planed_calls()
        {
            InitializeComponent();
            dbContext = new ApplicationContext1();
            List<Customer> customers = dbContext.Customers.ToList();
            ListOfCustomers.ItemsSource = customers;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.Show();
            Close();
        }

        private void ListOfCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, выбран ли элемент
            if (ListOfCustomers.SelectedItem != null)
            {
                // Получаем выбранный элемент
                Customer selectedCustomer = ListOfCustomers.SelectedItem as Customer;

                // Создаем новое окно CardWindow и передаем выбранного клиента в качестве параметра
                CardWindow cardWindow = new CardWindow(selectedCustomer, dbContext);

                // Открываем новое окно как диалоговое окно поверх текущего окна Planed_calls
                cardWindow.Owner = this;
                cardWindow.ShowDialog();
            }
        }
    }
}