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
        public static void SwitchTo(UserControl userControl)
        {
            MainWindowViewModel viewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            viewModel.CurrentControl = userControl;
        }
    }

}
