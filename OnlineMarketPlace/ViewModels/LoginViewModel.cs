using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineMarketPlace
{
    internal class LoginViewModel
    {
        public event RoutedEventHandler LoginSuccess;

        public void AuthenticateUser(string username, string password)
        {
            // Obviously now how real security is done but meh.
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
            if (sqlh.AuthenticateUser(username, password))
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



    }
}
