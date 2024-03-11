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
    /// Interaction logic for AdminPupControl.xaml
    /// </summary>
    public partial class AdminPupControl : UserControl
    {
        public AdminPupControl()
        {
            InitializeComponent();
        }

        private void ChoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TBD: change grid to employee grid
            // display employee stats?
            // (as per mvvm shove them into Employee?)

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AscDescButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SwitchToOrdersViewButton_Click(object sender, RoutedEventArgs e)
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
    }
}
