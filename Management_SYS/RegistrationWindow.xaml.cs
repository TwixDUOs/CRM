using System.Linq;
using System.Windows;
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
            AttachEnterKeyHandlers(); // Attach handlers for Enter key
        }

        // Attach handlers for Enter key to login, password, and password confirmation text boxes
        private void AttachEnterKeyHandlers()
        {
            textBoxLogin.KeyDown += TextBox_KeyDown;
            textBoxPassword.KeyDown += TextBox_KeyDown;
            textBoxPasswordConfirm.KeyDown += TextBox_KeyDown;
        }

        // Handle Enter key press event
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Registration_Click(this, new RoutedEventArgs());
            }
        }

        // Handle registration button click event
        private void Button_Registration_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;
            string passwordConfirm = textBoxPasswordConfirm.Password;

            // Validate input fields
            bool isLoginValid = ValidateLogin(login);
            bool isPasswordValid = ValidatePassword(password);
            bool isPasswordConfirmValid = ValidatePasswordConfirmation(password, passwordConfirm);

            // If all inputs are valid, register the user
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

        // Validate login input
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

        // Validate password input
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

        // Validate password confirmation input
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

        // Check if the input contains an uppercase letter
        private bool HasUpperCase(string input) => input.Any(char.IsUpper);

        // Check if the input contains a lowercase letter
        private bool HasLowerCase(string input) => input.Any(char.IsLower);

        // Check if the input contains a digit
        private bool HasDigit(string input) => input.Any(char.IsDigit);

        // Check if the input contains a special character
        private bool HasSpecialChar(string input) => input.Any(ch => !char.IsLetterOrDigit(ch));

        // Handle button click to open the authentication window
        private void ButtonWindowAuthentificationClick(object sender, RoutedEventArgs e)
        {
            AuthentificationWindow authentificationWindow = new AuthentificationWindow();
            authentificationWindow.Show();
            Close();
        }

        // Handle button click to close the application
        private void CloseApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
