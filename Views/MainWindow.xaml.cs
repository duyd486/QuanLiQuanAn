using QuanLiQuanAn.ViewModels;
using System.Windows;

namespace QuanLiQuanAn;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.DataContext = new MainViewModel();
        WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        InitializeComponent();
    }
}