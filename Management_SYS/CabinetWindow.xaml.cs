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
    /// <summary>
    /// Логика взаимодействия для CabinetWindow.xaml
    /// </summary>
    public partial class CabinetWindow : Window
    {
        ApplicationContext db;

        public CabinetWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();
        }

        private void HistoryLink_Click (object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.Show();
            this.Close();
        }

        private void PlannedCallsLink_Click (object sender, RoutedEventArgs e)
        {

            Planed_calls planed_Calls = new Planed_calls();
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
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        
    }
}
