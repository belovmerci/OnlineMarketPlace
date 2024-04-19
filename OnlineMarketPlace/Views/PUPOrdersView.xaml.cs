using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineMarketPlace
{
    /// <summary>
    /// Interaction logic for PUPOrdersView.xaml
    /// </summary>
    public partial class PupOrdersView : UserControl
    {
        public ObservableCollection<Product> Products { get; set; }
        public PupOrdersView()
        {
            // InitializeComponent();

            // method to retrieve products from the database
            Products = GetProducts();

            // Set the DataContext for data binding
            DataContext = new PUPOrdersViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AscDescButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {

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

        private ObservableCollection<Product> GetProducts()
        {
            // Implement logic to fetch products from the database and return them as ObservableCollection<Product>
            // For example, you can use Entity Framework or any other data access approach.
            return new ObservableCollection<Product>();
        }
    }
}
