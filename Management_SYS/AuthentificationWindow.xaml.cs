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
using System.Windows.Shapes;

namespace Management_SYS
{
    /// <summary>
    /// Логика взаимодействия для AuthentificationWindow.xaml
    /// </summary>
    public partial class AuthentificationWindow : Window
    {
        public AuthentificationWindow()
        {
            InitializeComponent();
        }


        private void Button_Authentification_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;

            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "It is too short";
                textBoxLogin.Background = Brushes.Red;
            }
            else if (password.Length < 5)
            {
                textBoxPassword.ToolTip = "It is too short";
                textBoxPassword.Background = Brushes.Red;
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                textBoxPassword.ToolTip = "";
                textBoxPassword.Background = Brushes.Transparent;

                bool isAnyUserRegistered = false;
                using (ApplicationContext db = new ApplicationContext())
                {
                    isAnyUserRegistered = db.Users.Any();
                }

                if (isAnyUserRegistered)
                {
                    User Authentificarion = null;
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        Authentificarion = db.Users.Where(b => b.Login == login && b.Password == password).FirstOrDefault();
                    }

                    if (Authentificarion != null)
                    {
                        CabinetWindow cabinetWindow = new CabinetWindow();
                        cabinetWindow.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Uncorrect login or password");
                    }
                }
                else
                {
                    MessageBox.Show("No users registered.");
                }
            }
        }
        private void ButtonRegistrationClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
