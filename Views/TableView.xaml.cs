using QuanLiQuanAn.ViewModels;
using System.Windows.Controls;

namespace QuanLiQuanAn.Views
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        public TableView()
        {
            this.DataContext = new TableViewModel();
            InitializeComponent();
        }
    }
}
