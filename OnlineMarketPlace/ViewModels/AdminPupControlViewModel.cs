using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineMarketPlace
{
    public class AdminPUPControlViewModel : ViewModelBase
    {
        private ObservableCollection<Product> _products;
        private ObservableCollection<PickUpPoint> _pickUpPoints;
        private PickUpPoint _selectedPickUpPoint;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand AscDescCommand { get; set; }
        public ICommand QuitCommand { get; set; }
        public ICommand SwitchToOrdersViewCommand { get; set; }

        public AdminPUPControlViewModel()
        {
            SearchCommand = new RelayCommand(SearchMethod);
            AscDescCommand = new RelayCommand(AscDescMethod);
            QuitCommand = new RelayCommand(QuitMethod);
            SwitchToOrdersViewCommand = new RelayCommand(SwitchToOrdersViewMethod);
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ObservableCollection<PickUpPoint> PickUpPoints
        {
            get => _pickUpPoints;
            set
            {
                _pickUpPoints = value;
                OnPropertyChanged(nameof(PickUpPoints));
            }
        }

        public PickUpPoint SelectedPickUpPoint
        {
            get => _selectedPickUpPoint;
            set
            {
                _selectedPickUpPoint = value;
                OnPropertyChanged(nameof(SelectedPickUpPoint));
                // Load data for the selected PickUpPoint here (e.g., employees, additional PickUpPoint information)
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
