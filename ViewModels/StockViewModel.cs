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
using OfficeOpenXml.Style;
using System.Drawing;
using QuanLiQuanAn.Singleton;

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
            QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(1000);
            IngredientOb.Clear();
            foreach (Ingredient ingredient in db.Ingredients.OrderByDescending(e => e.Id).Include(e => e.IngredientBills).ToList())
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
            MessageBoxResult message = MessageBox.Show("Delete this will delete all bill of this ingredient, are you sure you want to delete this?", "Yes", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(message == MessageBoxResult.Yes)
            {
                QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                foreach (IngredientBill ingredientBill in ingredient.IngredientBills)
                {
                    db.IngredientBills.Remove(ingredientBill);
                }
                db.Ingredients.Remove(ingredient);
                db.SaveChanges();
                db.Entry(ingredient).State = EntityState.Detached;
                _ = GetAll();
                MessageBox.Show("Delete Completed");
            }
        }

        [RelayCommand]
        [STAThread]
        private void ExportExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Ingredient");

                worksheet.Cells["A1:C1"].Merge = true;
                worksheet.Cells["A1"].Value = "Stock Information";
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                string[] headers = { "ID", "Name", "Quantity" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[2, i + 1].Value = headers[i];
                    worksheet.Cells[2, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[2, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[2, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(178, 61, 35));
                    worksheet.Cells[2, i + 1].Style.Font.Color.SetColor(Color.FromArgb(245, 213, 194));
                    worksheet.Cells[2, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Column(i + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Column(i + 1).Width = 30;
                }

                QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                int row = 3;
                foreach (Ingredient ingredient in db.Ingredients)
                {
                    var cell1 = worksheet.Cells[row, 1];
                    var cell2 = worksheet.Cells[row, 2];
                    var cell3 = worksheet.Cells[row, 3];
                    row++;
                    cell1.Value = ingredient.Id;
                    cell2.Value = ingredient.Name;
                    cell3.Value = ingredient.Quantity;

                }
                //worksheet.Cells.AutoFitColumns();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Chọn nơi lưu file Excel";
                saveFileDialog.FileName = "CurrentStock.xlsx";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                    MessageBox.Show($"File đã được lưu tại: {filePath}");
                }
                else
                {
                    MessageBox.Show("Hủy lưu file.");
                }
            }
        }



        private void UpdateIngredient()
        {
            try
            {
                QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (IngredientTmp.Name.IsNullOrEmpty() || IngredientTmp.Quantity == null)
                {
                    Exception exception = new Exception("Please fill all the blank");
                    throw exception;
                }
                db.Ingredients.Update(IngredientTmp);
                db.SaveChanges();
                db.Entry(IngredientTmp).State = EntityState.Detached;

                MessageBox.Show("Edit Completed!");
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
                QuanannhatContext db = DatabaseSingleton.GetInstance().db;
                IngredientTmp.Id = db.Ingredients.IsNullOrEmpty() ? 1 : db.Ingredients.OrderBy(e => e.Id).Last().Id + 1;

                if (IngredientTmp.Name.IsNullOrEmpty() || IngredientTmp.Quantity == null)
                {
                    Exception exception = new Exception("Please fill all the blank");
                    throw exception;
                }
                db.Ingredients.Add(IngredientTmp);
                db.SaveChanges();
                db.Entry(IngredientTmp).State = EntityState.Detached;

                MessageBox.Show("Add Completed!");
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
