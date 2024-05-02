using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace OnlineMarketPlace
{
    public class PUPOrdersViewModel : ViewModelBase
    {
        private ObservableCollection<Order> _orders;
        private string _searchText;
        public string OrdersSearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged(OrdersSearchText);
                Search(value);
            }
        }

        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Search(_searchText);
            }
        }

        public ICommand SearchCommand { get; private set; }
        public ICommand SortAscendingDescendingCommand { get; private set; }

        public PUPOrdersViewModel()
        {
            // Initialize commands
            SearchCommand = new RelayCommand(Search);
            SortAscendingDescendingCommand = new RelayCommand(SortAscendingDescending);

            // Initialize orders list
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
            Orders = sqlh.PullPUPOrders(NavHelp.MainWindowViewModel.EmployeeId);
            // GetAllOrdersForAllPickUpPoints();
        }

        private void Search(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If the search text is empty, display all orders
                RefreshOrders();
            }
            else
            {
                // Filter orders based on the search text
                var filteredOrders = _orders
                    .Where(order =>
                        order.CustomerName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        order.CreationDate.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        order.Price.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                Orders = new ObservableCollection<Order>(filteredOrders);
            }
        }

        private void SortAscendingDescending(object parameter)
        {
            var revOrders = Orders.Reverse();
            Orders.Clear();
            foreach (Order order in revOrders)
            {
                Orders.Add(order);
            }
        }

        private void RefreshOrders()
        {
            // Refresh orders list by fetching from the database again
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
            Orders = sqlh.PullPUPOrders(NavHelp.MainWindowViewModel.EmployeeId);
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Handle the edit ending event, if needed
            // For example, you can access the edited item and save changes to the database
            var editedProduct = e.Row.Item as Product;
            if (editedProduct != null)
            {
                // Implement logic to save changes to the database
            }
        }
    }
}