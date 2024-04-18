using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace OnlineMarketPlace
{
    public class AdminPUPControlViewModel : ViewModelBase
    {
        private ObservableCollection<Product> _products;
        private ObservableCollection<PickUpPoint> _pickUpPoints;
        private PickUpPoint _selectedPickUpPoint;
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

        public AdminPUPControlViewModel()
        {
            // Initialize commands or handle button clicks directly
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
                // Load data for the selected PickUpPoint here
                // (e.g., employees, additional PickUpPoint information)
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            // Handle search button click
        }

        public void AscDescButtonClick(object sender, RoutedEventArgs e)
        {
            // Handle Asc/Desc button click
        }

        public void QuitButtonClick(object sender, RoutedEventArgs e)
        {
            // Handle quit button click
        }

        public void SwitchToOrdersViewButtonClick(object sender, RoutedEventArgs e)
        {
            // Handle switch to orders view button click
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}