using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Management_SYS
{
    public partial class AuthentificationWindow : Window
    {
        public AuthentificationWindow()
        {
            InitializeComponent();
            AttachEnterKeyHandlers();
        }

        private void AttachEnterKeyHandlers()
        {
            textBoxLogin.KeyDown += TextBox_KeyDown;
            textBoxPassword.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Authenticate_Click(this, new RoutedEventArgs());
            }
        }

        private void Button_Authenticate_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;

            if (!IsValidInput(login, textBoxLogin, "It is too short") ||
                !IsValidInput(password, textBoxPassword, "It is too short"))
            {
                return;
            }

            ResetInputFields();

            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any())
                {
                    MessageBox.Show("No users registered.");
                    return;
                }

                User authenticatedUser = db.Users.FirstOrDefault(b => b.Login == login && b.Password == password);

                if (authenticatedUser != null)
                {
                    OpenCabinetWindow();
                }
                else
                {
                    MessageBox.Show("Incorrect login or password");
                }
            }
        }

        private bool IsValidInput(string input, Control control, string toolTip)
        {
            if (input.Length < 5)
            {
                control.ToolTip = toolTip;
                control.Background = Brushes.Red;
                return false;
            }
            return true;
        }

        private void ResetInputFields()
        {
            textBoxLogin.ToolTip = "";
            textBoxLogin.Background = Brushes.Transparent;
            textBoxPassword.ToolTip = "";
            textBoxPassword.Background = Brushes.Transparent;
        }

        private void OpenCabinetWindow()
        {
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.Show();
            Hide();
        }

        private void ButtonRegistrationClick(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
