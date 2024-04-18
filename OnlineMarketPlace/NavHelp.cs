using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OnlineMarketPlace
{
    public class NavHelp
    {
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


    }

}
