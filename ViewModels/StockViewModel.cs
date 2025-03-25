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
using QuanLiQuanAn.Views.Modals;
using System.Windows.Input;

namespace QuanLiQuanAn.ViewModels
{
    public partial class StockViewModel : ObservableObject
    {
        private ModalIngredientView modalIngredientView;
        private bool isEdit;

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
        }


        [RelayCommand]
        private void InteractIngredient(Ingredient ingredient)
        {
            isEdit = true;
            IngredientTmp = new();
            IngredientTmp = ingredient;

            modalIngredientView = new();
            modalIngredientView.DataContext = this;
            modalIngredientView.ShowDialog();
        }

        [RelayCommand]
        private void AddIngredient()
        {
            isEdit = false;

            IngredientTmp = new();

            modalIngredientView = new();
            modalIngredientView.DataContext = this;
            modalIngredientView.ShowDialog();
        }
        [RelayCommand]
        private void ApplyIngredient(string sender)
        {
            if (sender == "Apply")
            {
                if (!isEdit)
                {
                    SaveNewIngredient();
                }
                else
                {
                    UpdateIngredient();
                }
            }
            _ = GetAll();
            modalIngredientView.Close();
        }

        [RelayCommand]
        private void DeleteIngredient(Ingredient ingredient)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            db.Ingredients.Remove(ingredient);
            db.SaveChanges();
            db.Entry(ingredient).State = EntityState.Detached;
            _ = GetAll();
            MessageBox.Show("Delete Completed");

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
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (IngredientTmp.Name.ToString() == "")
                {
                    Exception exception = new Exception("Name is Null!!");
                    throw exception;
                }
                db.Ingredients.Update(IngredientTmp);
                db.SaveChanges();
                db.Entry(IngredientTmp).State = EntityState.Detached;

                MessageBox.Show("Sua thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SaveNewIngredient()
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
                IngredientTmp.Id = db.Ingredients.IsNullOrEmpty() ? 1 : db.Ingredients.OrderBy(e => e.Id).Last().Id + 1;

                if (IngredientTmp.Name.ToString() == "")
                {
                    Exception exception = new Exception("Name is emty");
                    throw exception;
                }
                db.Ingredients.Add(IngredientTmp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
