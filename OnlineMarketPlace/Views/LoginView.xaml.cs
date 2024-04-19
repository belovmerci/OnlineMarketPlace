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
    public partial class LoginView: UserControl
    {
        // Add event for login success
        public event RoutedEventHandler LoginSuccess;
        public RelayCommand LoginClick { get; private set; }

        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            LoginClick = new RelayCommand(param => Login_Click());
        }

        public void Login_Click()
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
                // lblErrorMessage.Text = "Invalid username or password";
            }
        }
        /*
        public void Login_Click(object sender, RoutedEventArgs e)
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
                // lblErrorMessage.Text = "Invalid username or password";
            }
         */


        private bool AuthenticateUser(string username, string password)
        {
            // Implement authentication logic here using T-SQL
            // Check credentials against the database

            // bool isValidUser = AuthenticationClass.CheckCredentials(username, password);
            bool isValidUser = true;

            return isValidUser;
        }
    }
}