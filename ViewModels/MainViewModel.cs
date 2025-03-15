using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using QuanLiQuanAn.DBContext;
using QuanLiQuanAn.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLiQuanAn.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
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
            switch(view)
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
