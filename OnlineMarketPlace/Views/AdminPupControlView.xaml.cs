using System.Windows.Controls;

namespace OnlineMarketPlace
{
    public partial class AdminPupControlView : UserControl
    {
        public AdminPupControlView()
        {
            DataContext = new AdminPUPControlViewModel();
            InitializeComponent();
        }
    }
}