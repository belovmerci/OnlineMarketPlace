using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineMarketPlace
{
    internal class LoginViewModel : ViewModelBase
    {

        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public RelayCommand LoginClick { get; private set; }

        public LoginViewModel()
        {
            LoginClick = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            // Access username and password from properties
            string username = Username;
            string password = Password;

            // Perform authentication logic here
            NavHelp.AuthenticateUser(username, password);
            NavHelp.MainWindowViewModel.username = username;
            NavHelp.MainWindowViewModel.password = password;
        }

    }
}
