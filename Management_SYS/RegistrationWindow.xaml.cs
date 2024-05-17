using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Management_SYS
{
    public partial class RegistrationWindow : Window
    {
        private readonly ApplicationContext db;

        public RegistrationWindow()
        {
            InitializeComponent();
            db = new ApplicationContext();
            AttachEnterKeyHandlers();
        }

        private void AttachEnterKeyHandlers()
        {
            textBoxLogin.KeyDown += TextBox_KeyDown;
            textBoxPassword.KeyDown += TextBox_KeyDown;
            textBoxPasswordConfirm.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Registration_Click(this, new RoutedEventArgs());
            }
        }

        private void Button_Registration_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;
            string passwordConfirm = textBoxPasswordConfirm.Password;

            bool isLoginValid = ValidateLogin(login);
            bool isPasswordValid = ValidatePassword(password);
            bool isPasswordConfirmValid = ValidatePasswordConfirmation(password, passwordConfirm);

            if (isLoginValid && isPasswordValid && isPasswordConfirmValid)
            {
                User user = new User(login, password);
                db.Users.Add(user);
                db.SaveChanges();

                AuthentificationWindow authentificationWindow = new AuthentificationWindow();
                authentificationWindow.Show();
                Close();
            }
        }

        private bool ValidateLogin(string login)
        {
            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "It is too short";
                textBoxLogin.Background = Brushes.Red;
                return false;
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                return true;
            }
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length < 5)
            {
                textBoxPassword.ToolTip = "It is too short";
                textBoxPassword.Background = Brushes.Red;
                return false;
            }
            if (!HasUpperCase(password))
            {
                textBoxPassword.ToolTip = "Password must contain at least one uppercase letter";
                textBoxPassword.Background = Brushes.Red;
                return false;
            }
            if (!HasLowerCase(password))
            {
                textBoxPassword.ToolTip = "Password must contain at least one lowercase letter";
                textBoxPassword.Background = Brushes.Red;
                return false;
            }
            if (!HasDigit(password))
            {
                textBoxPassword.ToolTip = "Password must contain at least one digit";
                textBoxPassword.Background = Brushes.Red;
                return false;
            }
            if (!HasSpecialChar(password))
            {
                textBoxPassword.ToolTip = "Password must contain at least one special character";
                textBoxPassword.Background = Brushes.Red;
                return false;
            }

            textBoxPassword.ToolTip = "";
            textBoxPassword.Background = Brushes.Transparent;
            return true;
        }

        private bool ValidatePasswordConfirmation(string password, string passwordConfirm)
        {
            if (passwordConfirm != password)
            {
                textBoxPasswordConfirm.ToolTip = "Confirm is not same to password";
                textBoxPasswordConfirm.Background = Brushes.Red;
                return false;
            }
            else
            {
                textBoxPasswordConfirm.ToolTip = "";
                textBoxPasswordConfirm.Background = Brushes.Transparent;
                return true;
            }
        }

        private bool HasUpperCase(string input) => input.Any(char.IsUpper);
        private bool HasLowerCase(string input) => input.Any(char.IsLower);
        private bool HasDigit(string input) => input.Any(char.IsDigit);
        private bool HasSpecialChar(string input) => input.Any(ch => !char.IsLetterOrDigit(ch));

        private void ButtonWindowAuthentificationClick(object sender, RoutedEventArgs e)
        {
            AuthentificationWindow authentificationWindow = new AuthentificationWindow();
            authentificationWindow.Show();
            Close();
        }

        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}