using QuanLiQuanAn.ViewModels;
using System.Windows.Controls;

namespace QuanLiQuanAn.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        public UserView()
        {
            this.DataContext = new UserViewModel();
            InitializeComponent();
        }
    }
}
