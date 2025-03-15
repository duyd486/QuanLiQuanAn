using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using QuanLiQuanAn.DBContext;
using QuanLiQuanAn.Models;
using System.Collections.ObjectModel;

namespace QuanLiQuanAn.ViewModels
{
    public partial class EmployeeViewModel : ObservableObject
    {
        private ObservableCollection<Employee> employeeOb;
        public IEnumerable<Employee> EmployeeOb => employeeOb;

        public ObservableCollection<string> SortItems { get; } = new()
        {
        "All", "Chef", "Cashier", "Waitress"
        };

        [ObservableProperty]
        private string? selectedSortItem;

        [ObservableProperty]
        private string? nameSearch = "";
        partial void OnNameSearchChanged(string? value)
        {
            SearchByName(value);
        }


        public EmployeeViewModel()
        {
            employeeOb = new ObservableCollection<Employee>();
            Console.WriteLine("Ctor");
        }



        [RelayCommand]
        private void GetAll()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            employeeOb.Clear();
            foreach (Employee employee in db.Employees.Include(e => e.Information).ToList())
            {
                employeeOb.Add(employee);
            }
        }
        [RelayCommand]
        private void SortRole(string roleStr)
        {
            SelectedSortItem = roleStr;
            int role = 0;
            switch (roleStr)
            {
                case "All":
                    GetAll();
                    return;
                case "Chef":
                    role = 1;
                    break;
                case "Cashier":
                    role = 2;
                    break;
                case "Waitress":
                    role = 3;
                    break;
            }
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            employeeOb.Clear();
            foreach (Employee employee in db.Employees.Include(e => e.Information).ToList())
            {
                if (employee.Role == role)
                {
                    employeeOb.Add(employee);
                }
            }
        }
        [RelayCommand]
        private void InteractEmployee(Information btn)
        {
            Console.WriteLine(btn.Name);
        }


        private void SearchByName(string name)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            employeeOb.Clear();
            foreach (Employee employee in db.Employees.Include(e => e.Information).ToList())
            {
                if (employee.Information.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    employeeOb.Add(employee);
                }
            }
        }

    }
}
