using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OnlineMarketPlace
{
    internal class PUPProductsViewModel : ViewModelBase
    {
        private ObservableCollection<Product> _products =
            new ObservableCollection<Product>();
        private ObservableCollection<Product> _originalProducts =
            new ObservableCollection<Product>();

        private string _searchText;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Search(_searchText);
            }
        }

        public ICommand SearchCommand { get; private set; }
        public ICommand SortAscendingDescendingCommand { get; private set; }
        public ICommand ShowOrdersViewCommand { get; private set; }

        public PUPProductsViewModel()
        {
            // Initialize commands
            SearchCommand = new RelayCommand(Search);
            SortAscendingDescendingCommand = new RelayCommand(SortAscendingDescending);
            ShowOrdersViewCommand = NavHelp.MainWindowViewModel.ShowOrdersViewCommand;

            // Initialize product list
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
            Products = sqlh.PullProductsByEmployeePUPs(NavHelp.MainWindowViewModel.EmployeeId);
            _originalProducts = Products;
        }

        private void Search(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If the search text is empty, display all products
                Products = _originalProducts;
            }
            else
            {
                // Filtering _originalProducts to reset
                ObservableCollection<Product> filteredProducts =
                    new ObservableCollection<Product>(_originalProducts.Where(product =>
                        product.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        product.Description.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0));

                Products = filteredProducts;
            }
        }

        private void SortAscendingDescending(object parameter)
        {
            // Products = new ObservableCollection<Product>(Products.OrderBy(product => product.Name));
            var revProducts = Products.Reverse();
            Products.Clear();
            foreach (Product product in revProducts)
            {
                Products.Add(product);
            }
        }

        private void SaveChanges(object parameter)
        {
            // Create lists to hold products to add, update, and delete
            ObservableCollection<Product> productsToAdd = new ObservableCollection<Product>();
            ObservableCollection<Product> productsToUpdate = new ObservableCollection<Product>();
            ObservableCollection<Product> productsToDelete = new ObservableCollection<Product>();

            // Iterate through the Products collection
            foreach (var product in Products)
            {
                if (product.Id == 0)
                {
                    // If the product's ID is 0, it's a new product
                    productsToAdd.Add(product);
                }
                else
                {
                    // Check if the product exists in the original collection
                    var originalProduct = _originalProducts.FirstOrDefault(p => p.Id == product.Id);
                    if (originalProduct == null)
                    {
                        // If the product doesn't exist in the original collection, it's a new product
                        productsToAdd.Add(product);
                    }
                    else if (!product.Equals(originalProduct))
                    {
                        // If the product is different from the original, it needs to be updated
                        productsToUpdate.Add(product);
                    }
                }
            }

            // Check for products to delete (products that exist in the original collection but not in the current collection)
            foreach (var originalProduct in _originalProducts)
            {
                if (!Products.Any(p => p.Id == originalProduct.Id))
                {
                    productsToDelete.Add(originalProduct);
                }
            }

            // Save changes to database
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
            sqlh.SaveProductChanges(productsToAdd, productsToUpdate, productsToDelete);

            // Re-Pull the employee products collection
            _originalProducts.Clear();
            foreach (var product in sqlh.PullProductsByEmployeePUPs(
                                NavHelp.MainWindowViewModel.EmployeeId))
            {
                Products.Add(product);
            }
        }
    }
}