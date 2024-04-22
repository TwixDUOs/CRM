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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;



        public MainWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();
        }

        private void Button_Registration_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;
            string passwordConfirm = textBoxPasswordConfirm.Password;

            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "It is too short";
                textBoxLogin.Background = Brushes.Red;
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
            }
            if (password.Length < 5)
            {
                textBoxPassword.ToolTip = "It is too short";
                textBoxPassword.Background = Brushes.Red;
            }
            else
            {
                textBoxPassword.ToolTip = "";
                textBoxPassword.Background = Brushes.Transparent;
            }
            if (passwordConfirm.Length < 5)
            {
                textBoxPasswordConfirm.ToolTip = "It is too short";
                textBoxPasswordConfirm.Background = Brushes.Red;
            }
            else
            {
                textBoxPasswordConfirm.ToolTip = "";
                textBoxPasswordConfirm.Background = Brushes.Transparent;
            }
            if (passwordConfirm != password)
            {
                textBoxPasswordConfirm.ToolTip = "Confirm is not same to password";
                textBoxPasswordConfirm.Background = Brushes.Red;
            }
            else
            {
                textBoxPasswordConfirm.ToolTip = "";
                textBoxPasswordConfirm.Background = Brushes.Transparent;
            }
            if (login.Length >= 5 && password.Length >= 5 && passwordConfirm.Length >= 5 && passwordConfirm == password)
            {
                textBoxPasswordConfirm.ToolTip = "";
                textBoxPasswordConfirm.Background = Brushes.Transparent;
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                textBoxPassword.ToolTip = "";
                textBoxPassword.Background = Brushes.Transparent;

                User user = new User(login, password);
                db.Users.Add(user);
                db.SaveChanges();

                AuthentificationWindow authentificationWindow = new AuthentificationWindow();
                authentificationWindow.Show();
                this.Close();
            }


        }

        private void ButtonWindowAuthentificationClick(object sender, RoutedEventArgs e)
        {
            AuthentificationWindow authentificationWindow = new AuthentificationWindow();
            authentificationWindow.Show();
            this.Close();
        }

        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
