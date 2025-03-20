using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using QuanLiQuanAn.DBContext;
using QuanLiQuanAn.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace QuanLiQuanAn.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Dish> dishOb;

        [ObservableProperty]
        private ObservableCollection<Dishlist> dishlistOb;

        public ObservableCollection<string> SortItems { get; } =
        [
        "All"
        ];

        [ObservableProperty]
        private Visibility isLoading;


        [ObservableProperty]
        private string? selectedSortItem;

        [ObservableProperty]
        private string? nameSearch = "";
        partial void OnNameSearchChanged(string? value)
        {
            SearchByName(value);
        }


        public MenuViewModel()
        {
            DishOb = new ObservableCollection<Dish>();
            dishlistOb = new ObservableCollection<Dishlist>();
            _ = Loading();
            SelectedSortItem = "All";
        }


        private async Task Loading()
        {
            IsLoading = Visibility.Visible;

            await GetAll();

            IsLoading = Visibility.Hidden;

        }


        [RelayCommand]
        private async Task GetAll()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(1000);
            DishOb.Clear();
            foreach (Dish dish in db.Dishes.OrderByDescending(e => e.Id))
            {
                DishOb.Add(dish);
            }
            DishlistOb.Clear();
            SortItems.Clear();
            SortItems.Add("All");
            foreach (Dishlist dishlist in db.Dishlists.OrderByDescending(e => e.Id))
            {
                DishlistOb.Add(dishlist);
                SortItems.Add(dishlist.Name);
            }
            SelectedSortItem = "All";
        }
        [RelayCommand]
        private void SortDish(string dishStr)
        {
            SelectedSortItem = dishStr;
            if (dishStr == "All")
            {
                _ = GetAll();
                return;
            }
            else
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
                DishOb.Clear();
                foreach(Dishlist dishlist in DishlistOb.Where(e => e.Name.Equals(dishStr)))
                {
                    foreach(Dish dish in dishlist.Dishes)
                    {
                        DishOb.Add(dish);
                    }
                }
            }
        }


        [ObservableProperty]
        Dish? dishTmp;
        [ObservableProperty]
        Dishlist dishlistTmp;
        [ObservableProperty]
        //private DateOnly date = DateOnly.FromDateTime(DateTime.Today);
        //private Views.AddEmployee addEmployeeView;
        private bool isEdit;
        [RelayCommand]
        private void InteractDish(Dish dish)
        {
            //isEdit = true;
            //EmployeeTmp = new();
            //EmployeeTmp = employee;
            //InformationTmp = new();
            //InformationTmp = employee.Information;

            //addEmployeeView = new();
            //addEmployeeView.DataContext = this;
            //addEmployeeView.ShowDialog();
        }

        [RelayCommand]
        private void AddDish()
        {
            //isEdit = false;
            //InformationTmp = new();
            //EmployeeTmp = new();
            //addEmployeeView = new();
            //addEmployeeView.DataContext = this;
            //addEmployeeView.ShowDialog();
        }
        [RelayCommand]
        private void ApplyDish(string sender)
        {
            //if (sender == "Apply")
            //{
            //    if (!isEdit)
            //    {
            //        SaveNewEmployee();
            //    }
            //    else
            //    {
            //        UpdateEmployee();
            //    }
            //}
            //_ = GetAll();
            //addEmployeeView.Close();
        }

        [RelayCommand]
        private void DeleteDish(Dish dish)
        {
            //EmployeeTmp = new();
            //EmployeeTmp = employee;
            //QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            //SalaryBillOb.Clear();
            //SalaryBillOb = (List<SalaryBill>)EmployeeTmp.SalaryBills;
            //foreach (SalaryBill salaryBill in SalaryBillOb)
            //{
            //    Console.WriteLine(salaryBill.EmployeeId);
            //}
            //Console.WriteLine("Delete" + EmployeeTmp.Id);

        }

        [RelayCommand]
        private void ImportExcel()
        {
            //string filePath = "";
            //OpenFileDialog openFileDialog = new OpenFileDialog();

            //if (openFileDialog.ShowDialog() == true)
            //{
            //    filePath = openFileDialog.FileName;
            //}
            //if (filePath == "")
            //{
            //    return;
            //}
            //try
            //{
            //    QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();

            //    var package = new ExcelPackage(new FileInfo(filePath));
            //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            //    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            //    {
            //        SalaryBill salaryBill = new SalaryBill();
            //        salaryBill.Id = db.SalaryBills.IsNullOrEmpty() ? 1 : db.SalaryBills.OrderBy(e => e.Id).Last().Id + 1;

            //        int employeeIdCollum = 1;
            //        salaryBill.EmployeeId = Convert.ToInt32(worksheet.Cells[i, employeeIdCollum].Value);

            //        int totalShiftCollum = 3;
            //        salaryBill.TotalShifts = Convert.ToInt32(worksheet.Cells[i, totalShiftCollum].Value);

            //        salaryBill.Status = 1;

            //        int timeCollum = 7;
            //        string? timeStr = worksheet.Cells[i, timeCollum].Value.ToString();
            //        salaryBill.Time = DateOnly.Parse(timeStr.Substring(0, timeStr.IndexOf(" ")));

            //        db.SalaryBills.Add(salaryBill);
            //        db.SaveChanges();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }



        private void UpdateDish()
        {
            //try
            //{
            //    QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            //    if (EmployeeTmp.Salary.ToString() == "")
            //    {
            //        Exception exception = new Exception("Salary was write in wrong condiction");
            //        throw exception;
            //    }
            //    else if (InformationTmp.CitizenId.ToString() == "")
            //    {
            //        Exception exception = new Exception("Cityzen Id was write in wrong condiction");
            //        throw exception;
            //    }
            //    else if (InformationTmp.Name == "")
            //    {
            //        Exception exception = new Exception("Name is emty");
            //        throw exception;
            //    }

            //    db.Informations.Update(InformationTmp);
            //    db.Employees.Update(EmployeeTmp);
            //    db.SaveChanges();
            //    db.Entry(InformationTmp).State = EntityState.Detached;

            //    MessageBox.Show("Sua thanh cong");

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }

        private void SaveNewDish()
        {
            //try
            //{
            //    QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            //    InformationTmp.Id = db.Informations.IsNullOrEmpty() ? 1 : db.Informations.OrderBy(e => e.Id).Last().Id + 1;
            //    EmployeeTmp.Id = db.Employees.IsNullOrEmpty() ? 1 : db.Employees.OrderBy(e => e.Id).Last().Id + 1;
            //    EmployeeTmp.InformationId = InformationTmp.Id;
            //    InformationTmp.Birth = Date;

            //    if (EmployeeTmp.Salary.ToString() == "")
            //    {
            //        Exception exception = new Exception("Salary was write in wrong condiction");
            //        throw exception;
            //    }
            //    else if (InformationTmp.CitizenId.ToString() == "")
            //    {
            //        Exception exception = new Exception("Cityzen Id was write in wrong condiction");
            //        throw exception;
            //    }
            //    else if (InformationTmp.Name == "")
            //    {
            //        Exception exception = new Exception("Name is emty");
            //        throw exception;
            //    }

            //    db.Informations.Add(InformationTmp);
            //    db.Employees.Add(EmployeeTmp);
            //    db.SaveChanges();
            //    MessageBox.Show("Them thanh cong");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }

        private void SearchByName(string name)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            DishOb.Clear();
            foreach (Dish dish in db.Dishes)
            {
                if (dish.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    DishOb.Add(dish);
                }
            }
        }

    }
}
