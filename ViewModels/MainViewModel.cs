using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLiQuanAn.DBContext;
using System.Collections.ObjectModel;

namespace QuanLiQuanAn.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            ChangeView("Dashboard");
        }


        public ObservableCollection<string> MenuItems { get; } = new()
        {
        "Dashboard", "Menu", "Table", "Bill", "Employee", "Customer", "Stock"
        };

        [ObservableProperty]
        public string selectedMenuItem;

        [ObservableProperty]
        public object currentViewModel;

        [RelayCommand]
        private void ChangeView(string view)
        {
            SelectedMenuItem = view;
            switch (view)
            {
                case "Dashboard":
                    CurrentViewModel = new DashboardViewModel();
                    break;
                case "Menu":
                    CurrentViewModel = new MenuViewModel();
                    break;
                case "Table":
                    CurrentViewModel = new TableViewModel();
                    break;
                case "Bill":
                    CurrentViewModel = new BillViewModel();
                    break;
                case "Employee":
                    CurrentViewModel = new EmployeeViewModel();
                    break;
                case "Customer":
                    CurrentViewModel = new UserViewModel();
                    break;
                case "Stock":
                    CurrentViewModel = new StockViewModel();
                    break;
            }
        }

    }
}
