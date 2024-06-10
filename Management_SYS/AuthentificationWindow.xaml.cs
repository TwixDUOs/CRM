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
            AttachEnterKeyHandlers(); // Attach handlers for Enter key press
        }

        private void AttachEnterKeyHandlers()
        {
            // Attach KeyDown event handlers to textBoxLogin and textBoxPassword
            textBoxLogin.KeyDown += TextBox_KeyDown;
            textBoxPassword.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Trigger authentication button click on Enter key press
            if (e.Key == Key.Enter)
            {
                Button_Authenticate_Click(this, new RoutedEventArgs());
            }
        }

        private void Button_Authenticate_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input from text boxes
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;

            // Validate user input
            if (!IsValidInput(login, textBoxLogin, "It is too short") ||
                !IsValidInput(password, textBoxPassword, "It is too short"))
            {
                return;
            }

            ResetInputFields(); // Reset input field styles

            using (ApplicationContext db = new ApplicationContext())
            {
                // Check if there are any registered users
                if (!db.Users.Any())
                {
                    MessageBox.Show("No users registered.");
                    return;
                }

                // Authenticate user
                User authenticatedUser = db.Users.FirstOrDefault(b => b.Login == login && b.Password == password);

                if (authenticatedUser != null)
                {
                    OpenCabinetWindow(); // Open CabinetWindow if authentication is successful
                }
                else
                {
                    MessageBox.Show("Incorrect login or password"); // Show error message if authentication fails
                }
            }
        }

        private bool IsValidInput(string input, Control control, string toolTip)
        {
            // Check if the input length is valid
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
            // Reset styles of the input fields
            textBoxLogin.ToolTip = "";
            textBoxLogin.Background = Brushes.Transparent;
            textBoxPassword.ToolTip = "";
            textBoxPassword.Background = Brushes.Transparent;
        }

        private void OpenCabinetWindow()
        {
            // Open CabinetWindow and hide the current window
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.Show();
            Hide();
        }

        private void ButtonRegistrationClick(object sender, RoutedEventArgs e)
        {
            // Open RegistrationWindow and close the current window
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
