using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace OnlineMarketPlace
{
    public class AdminPUPControlViewModel : ViewModelBase
    {
        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        private ObservableCollection<Employee> _employeesToAdd = new ObservableCollection<Employee>();
        private ObservableCollection<Employee> _employeesToDelete = new ObservableCollection<Employee>();

        private ObservableCollection<Product> _products;
        private ObservableCollection<PickUpPoint> _pickUpPoints;
        public ObservableCollection<PickUpPoint> PUPs
        {
            get => _pickUpPoints;
            set
            {
                _pickUpPoints = value;
                OnPropertyChanged(nameof(PUPs));
            }
        }

        private PickUpPoint _selectedPup = new PickUpPoint();
        public PickUpPoint SelectedPUP
        {
            get => _selectedPup;
            set
            {
                _selectedPup = value;
                OnPropertyChanged(nameof(SelectedPUP));

                // Check if the selected PUP is "Select All" or a specific PUP
                if (value != null && value.Id == 0)
                {
                    // Load all employees
                    Employees = _originalEmployees;
                }
                else
                {
                    // Load employees for the selected PUP
                    LoadEmployeesByPUP(value);
                }
            }
        }

        private ObservableCollection<Employee> _originalEmployees;

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
                    Search();
                }
            }
        }

        // public ICommand SearchButtonClick { get; private set; }
        public ICommand AscDescButtonClick { get; private set; }
        public ICommand ShowAdminPupProductsViewCommand { get; private set; }
        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }


        public AdminPUPControlViewModel()
        {
            AscDescButtonClick = new RelayCommand(AscDescButton);
            ShowAdminPupProductsViewCommand =
                NavHelp.MainWindowViewModel.ShowAdminPupProductsViewCommand;
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            SaveChangesCommand = new RelayCommand(SaveChanges);

            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();

            PickUpPoints = new ObservableCollection<PickUpPoint>
            {
                // Option to select all
                new PickUpPoint { Id = 0, Street = "Выбрать все" }
            };
            SelectedPUP = PickUpPoints.First();
            foreach (PickUpPoint pup in sqlh.PullAllPUPs()) PickUpPoints.Add(pup);

            _originalEmployees = sqlh.PullAllEmployees();

            Employees = _originalEmployees;
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

        public void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If the search text is empty, display all employees in original order
                Employees = new ObservableCollection<Employee>(_originalEmployees);
            }
            else
            {
                // Filter employees based on the search text
                var filteredEmployees = _originalEmployees
                    .Where(employee =>
                        employee.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        employee.Surname.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        employee.FathersName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        employee.DateOfBirth.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        employee.Wage.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        employee.Rating.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                filteredEmployees.Reverse();

                Employees = new ObservableCollection<Employee>(filteredEmployees);
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

        private void AscDescButton(object obj)
        {
            Employees.Reverse();
        }

        private void AddProduct(object obj)
        {
            Employee newEmployee = obj as Employee;
            if (newEmployee != null)
            {
                Employees.Add(newEmployee);
                _employeesToAdd.Add(newEmployee);
            }
        }

        private void DeleteProduct(object obj)
        {
            Employee newEmployee = obj as Employee;
            if (newEmployee != null)
            {
                Employees.Add(newEmployee);
                _employeesToDelete.Add(newEmployee);
            }
        }

        private void SaveChanges(object obj)
        {
            SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
            sqlh.SaveEmployeeChanges(_employeesToAdd, _employeesToDelete);

            _employeesToAdd.Clear();
            _employeesToDelete.Clear();
        }

        private void LoadEmployeesByPUP(PickUpPoint selectedPUP)
        {
            if (selectedPUP == null) return;

            // Load employees based on the selected PUP
            if (selectedPUP.Id >= 0)
            {
                SqlDatabaseHelper sqlh = new SqlDatabaseHelper();
                if (selectedPUP.Id == 0)
                {
                    // Load all employees
                    Employees = sqlh.PullAllEmployees();
                }
                else
                {
                    // Load employees for the selected PUP
                    Employees = sqlh.PullEmployeesByPUP(selectedPUP.Id);
                }
                _originalEmployees = Employees;
            }
        }
    }
}
