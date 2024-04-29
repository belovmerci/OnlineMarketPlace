using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;

namespace OnlineMarketPlace
{
    public class AdminPUPProductsViewModel : ViewModelBase
    {

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private ObservableCollection<Product> _productsToAdd = new ObservableCollection<Product>();
        private ObservableCollection<Product> _productsToUpdate = new ObservableCollection<Product>();
        private ObservableCollection<Product> _productsToDelete = new ObservableCollection<Product>();

        // Commands
        public ICommand DataGridRowEditEndingCommand { get; set; }
        public ICommand ShowAdminPupControlViewCommand { get; private set; }
        public ICommand SwitchToPUPViewCommand { get; private set; }
        public ICommand AscDescButtonCommand { get; private set; }
        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }

        public AdminPUPProductsViewModel()
        {
            DataGridRowEditEndingCommand =
                new RelayCommandT<DataGridRowEditEndingEventArgs>(DataGridRowEditEnding);
            ShowAdminPupControlViewCommand = NavHelp.MainWindowViewModel.ShowAdminPupControlViewCommand;
            SwitchToPUPViewCommand = NavHelp.MainWindowViewModel.ShowAdminPupControlViewCommand;
            AscDescButtonCommand = new RelayCommand(AscDescButton);
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            SaveChangesCommand = new RelayCommand(SaveChanges);

            Products = new ObservableCollection<Product>();
            _productsToAdd = new ObservableCollection<Product>();
            _productsToDelete = new ObservableCollection<Product>();
        }

        // Command methods

        private void DataGridRowEditEnding(DataGridRowEditEndingEventArgs e)
        {
            // Check if the edit is a new item being added
            if (e.EditAction == DataGridEditAction.Commit && e.Row.IsNewItem)
            {
                // Add the new item to the collection
                Product newProduct = e.Row.Item as Product;
                if (newProduct != null)
                {
                    Products.Add(newProduct);
                    _productsToAdd.Add(newProduct);
                }
            }

            bool allFieldsBlank = true;
            Product editedProduct = e.Row.Item as Product;
            if (editedProduct != null)
            {
                // Check each property of the Product class for blankness
                if (!string.IsNullOrWhiteSpace(editedProduct.Name))
                    allFieldsBlank = false;
                if (!string.IsNullOrWhiteSpace(editedProduct.Description))
                    allFieldsBlank = false;
                if (editedProduct.Price != 0)
                    allFieldsBlank = false;
                if (editedProduct.Rating != 0)
                    allFieldsBlank = false;
                if (editedProduct.Amount != 0)
                    allFieldsBlank = false;

                if (allFieldsBlank)
                {
                    Products.Remove(e.Row.Item as Product);
                }
            }
        }

        private void AscDescButton(object obj)
        {
            Products.Reverse();
        }

        private void AddProduct(object obj)
        {
            Product newProduct = obj as Product;
            if (newProduct != null)
            {
                Products.Add(newProduct);
                _productsToAdd.Add(newProduct);
            }
        }

        private void DeleteProduct(object obj)
        {
            Product productToDelete = obj as Product;
            if (productToDelete != null)
            {
                Products.Remove(productToDelete);
                _productsToDelete.Add(productToDelete);
            }
        }

        private void SaveChanges(object obj)
        {
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
            sqlh.SaveProductChanges(_productsToAdd, _productsToUpdate, _productsToDelete);

            _productsToAdd.Clear();
            _productsToDelete.Clear();
        }
    }
}
