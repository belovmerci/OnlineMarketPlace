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
        public event RoutedEventHandler LoginSuccess;
        public RelayCommand LoginClick { get; private set; }

        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            LoginClick = new RelayCommand(
                param => AuthenticateUser(txtUsername.Text,
                                          txtPassword.Password));
        }

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
                lblErrorMessage.Text = "Invalid username or password";
            }
        }
    }
}