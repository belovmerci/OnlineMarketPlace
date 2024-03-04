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

namespace OnlineMarketPlace
{
    public partial class LoginUserControl : UserControl
    {
        // Add event for login success
        public event RoutedEventHandler LoginSuccess;

        public LoginUserControl()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Implement authentication logic using T-SQL database
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Example: Replace this with your actual authentication logic
            bool isAuthenticated = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                // Trigger the LoginSuccess event
                LoginSuccess?.Invoke(this, new RoutedEventArgs());
            }
            else
            {
                lblErrorMessage.Text = "Invalid username or password";
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            // Implement your authentication logic here using T-SQL
            // Example: Check credentials against the database

            // Replace the following line with your actual database check
            // bool isValidUser = YourAuthenticationClass.CheckCredentials(username, password);
            bool isValidUser = true;

            return isValidUser;
        }
    }
}