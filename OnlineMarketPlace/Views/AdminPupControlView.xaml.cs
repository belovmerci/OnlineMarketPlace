using System.Windows.Controls;

namespace OnlineMarketPlace
{
    public partial class AdminPupControl : UserControl
    {
        public AdminPupControl()
        {
            InitializeComponent();
            DataContext = new AdminPUPControlViewModel();
        }
    }
}