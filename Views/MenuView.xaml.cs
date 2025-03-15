using QuanLiQuanAn.ViewModels;
using System.Windows.Controls;

namespace QuanLiQuanAn.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            this.DataContext = new MenuViewModel();
            InitializeComponent();
        }
    }
}
