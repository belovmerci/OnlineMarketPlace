using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

namespace OnlineMarketPlace
{
    public class MainWindowViewModel : ViewModelBase
    {
        private UserControl _currentControl;

        public UserControl CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;
                OnPropertyChanged(nameof(CurrentControl));
            }
        }

        public MainWindowViewModel()
        {
            // Set LoginUserControl as the initial control
            CurrentControl = new LoginUserControl();
        }
    }
}
