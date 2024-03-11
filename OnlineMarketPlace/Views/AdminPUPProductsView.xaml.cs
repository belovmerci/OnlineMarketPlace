using System;
using System.Collections.Generic;
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
    /// Interaction logic for AdminViewProducts.xaml
    /// </summary>
    public partial class AdminPUPProductsView : UserControl
    {
        public AdminPUPProductsView()
        {
            InitializeComponent();
            // init adminpupproducts viewmodel
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

        private void SwitchToPUPViewButton_Click(object sender, RoutedEventArgs e)
        {
            NavHelp.SwitchTo(new ViewProducts());
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
