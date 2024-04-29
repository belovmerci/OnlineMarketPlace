using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OnlineMarketPlace
{
    public static class NavHelp
    {
        private static MainWindowViewModel _mainWindowViewModel;
        public static MainWindowViewModel MainWindowViewModel
        {
            get
            {
                if (_mainWindowViewModel == null)
                {
                    _mainWindowViewModel = new MainWindowViewModel();
                }
                return _mainWindowViewModel;
            }
        }

        /*
        private UserControl _currentControl;
        public UserControl CurrentControl
        {
            get
            {
                return _currentControl;
            }
            set
            {
                MainWindowViewModel viewModel =
                    (MainWindowViewModel)Application.Current.MainWindow.DataContext;
                viewModel.CurrentControl = value;
            }
        }

            public RelayCommand ShowLoginViewCommand { () =>  }
            public RelayCommand ShowAdminViewProductsCommand { () => }
            public RelayCommand ShowAdminPupControlViewCommand { () => }
            public RelayCommand ShowAdminPupProductsViewCommand { () => }
        */


        public static void SwitchTo(UserControl userControl)
        {
            MainWindowViewModel viewModel =
                (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            viewModel.CurrentControl = userControl;
        }

        public static void AuthenticateUser(string username, string password)
        {
            // Obviously now how real security is done but eh.
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();

            if (sqlh.AuthenticateUser(username, password))
            {
                NavHelp.MainWindowViewModel.username = username;
                NavHelp.MainWindowViewModel.password = password;
            }
            else
            {
                Console.WriteLine("ERROR!");
            }
        }
    }
}
