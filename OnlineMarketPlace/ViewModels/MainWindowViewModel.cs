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

        public string username;
        public string password;
        public bool IsAdmin;
        public int EmployeeId;

        public RelayCommand ShowLoginViewCommand { get; private set; }
        public RelayCommand ShowViewProductsCommand { get; private set; }
        public RelayCommand ShowOrdersViewCommand { get; private set; }
        public RelayCommand ShowAdminPupControlViewCommand { get; private set; }
        public RelayCommand ShowAdminPupProductsViewCommand { get; private set; }

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
            ShowLoginViewCommand = new RelayCommand(param => ShowLoginView());
            ShowViewProductsCommand = new RelayCommand(param => ShowProductsView());
            ShowOrdersViewCommand = new RelayCommand(param => ShowOrdersView());
            ShowAdminPupControlViewCommand = new RelayCommand(param => ShowAdminPUPControlView());
            ShowAdminPupProductsViewCommand = new RelayCommand(param => ShowAdminPUPProductsView());

            // Set LoginUserControl as the initial control
            CurrentControl = new LoginView();
        }

        private void ShowLoginView()
        {
            CurrentControl = new LoginView();
        }

        private void ShowProductsView()
        {
            CurrentControl = new PUPProductsView();
        }
        
        private void ShowOrdersView()
        {
            CurrentControl = new PupOrdersView();
        }

        private void ShowAdminPUPControlView()
        {
            CurrentControl = new AdminPupControlView();
        }
        private void ShowAdminPUPProductsView()
        {
            CurrentControl = new AdminPupProductsView();
        }
    }
}
