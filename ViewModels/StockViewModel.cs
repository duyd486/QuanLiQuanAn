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
using QuanLiQuanAn.Views;

namespace QuanLiQuanAn.ViewModels
{
    public partial class StockViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<Ingredient> ingredientOb;
        [ObservableProperty] private Visibility isLoading;
        [ObservableProperty] private string? selectedSortItem;
        [ObservableProperty] private string? nameSearch = "";
        [ObservableProperty] Ingredient? ingredientTmp;


        public ObservableCollection<string> SortItems { get; } =
        [
        "All"
        ];


        partial void OnNameSearchChanged(string? value)
        {
            SearchByName(value);
        }


        public StockViewModel()
        {
            IngredientOb = new ObservableCollection<Ingredient>();
            _ = Loading();
            selectedSortItem = "All";
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
            IngredientOb.Clear();
            foreach (Ingredient ingredient in db.Ingredients.OrderByDescending(e => e.Id))
            {
                IngredientOb.Add(ingredient);
            }
        }
        [RelayCommand]
        private void SortIngredient(string ingredientStr)
        {
            SelectedSortItem = ingredientStr;
            switch (ingredientStr)
            {
                case "All":
                    _ = GetAll();
                    return;
            }
            //QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            //DishOb.Clear();
            
            //foreach (Dish dish in DishlistOb)
            //{
                
            //}
        }


        //[ObservableProperty]
        //private DateOnly date = DateOnly.FromDateTime(DateTime.Today);
        private AddIngredientView addIngredientView;
        private bool isEdit;
        [RelayCommand]
        private void InteractIngredient(Ingredient ingredient)
        {
            isEdit = true;
            //EmployeeTmp = new();
            //EmployeeTmp = employee;
            //InformationTmp = new();
            //InformationTmp = employee.Information;

            //addEmployeeView = new();
            //addEmployeeView.DataContext = this;
            //addEmployeeView.ShowDialog();
        }

        [RelayCommand]
        private void AddIngredient()
        {
            //isEdit = false;
            //InformationTmp = new();
            //EmployeeTmp = new();
            //addEmployeeView = new();
            //addEmployeeView.DataContext = this;
            //addEmployeeView.ShowDialog();
        }
        [RelayCommand]
        private void ApplyIngredient(string sender)
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
        private void DeleteIngredient(Ingredient ingredient)
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



        private void UpdateIngredient()
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

        private void SaveNewIngredient()
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
            SortIngredient(SelectedSortItem);
            for (int i = 0; i < IngredientOb.Count; i++)
            {
                if (!IngredientOb[i].Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    IngredientOb.Remove(IngredientOb[i]);
                }
            }
        }

    }
}
