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
using QuanLiQuanAn.Views;
using System.Windows.Input;

namespace QuanLiQuanAn.ViewModels
{
    public partial class TableViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Table> tableOb;

        public ObservableCollection<string> SortItems { get; } =
        [
        "All", "Table For 2", "Table For 3", "Table For 4", "Above 4"
        ];

        [ObservableProperty]
        private Visibility isLoading;

        public IEnumerable<string> Gender { get; } =
        [
            "Male" , "Female"
        ];

        [ObservableProperty]
        private string? selectedSortItem;



        public TableViewModel()
        {
            TableOb = new ObservableCollection<Table>();
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
            TableOb.Clear();
            foreach (Table table in db.Tables.OrderByDescending(e => e.Id))
            {
                TableOb.Add(table);
            }

        }
        [RelayCommand]
        private void SortContain(string containStr)
        {
            SelectedSortItem = containStr;
            int contain = 0;
            switch (containStr)
            {
                case "All":
                    _ = GetAll();
                    return;
                case "Table For 2":
                    contain = 2;
                    break;
                case "Table For 3":
                    contain = 3;
                    break;
                case "Table For 4":
                    contain = 4;
                    break;
                default:
                    contain = 0;
                    break;
            }
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            TableOb.Clear();
            foreach (Table table in db.Tables)
            {
                if(contain != 0)
                {
                    if (table.Contain == contain)
                    {
                        TableOb.Add(table);
                    }
                } else
                {
                    if(table.Contain > 4)
                    {
                        TableOb.Add(table);
                    }
                }
                

            }
        }


        [ObservableProperty]
        Table? tableTmp;
        private AddTableView addTableView;
        private bool isEdit;
        [RelayCommand]
        private void InteractTable(Table table)
        {
            isEdit = true;
            TableTmp = new();
            TableTmp = table;

            addTableView = new();
            addTableView.DataContext = this;
            addTableView.ShowDialog();
        }

        [RelayCommand]
        private void AddTable()
        {
            isEdit = false;
            TableTmp = new();
            addTableView = new();
            addTableView.DataContext = this;
            addTableView.ShowDialog();
        }
        [RelayCommand]
        private void ApplyTable(string sender)
        {
            if (sender == "Apply")
            {
                if (!isEdit)
                {
                    SaveNewTable();
                    _ = GetAll();
                    MessageBox.Show("Them thanh cong");
                }
                else
                {
                    UpdateTable();
                    _ = GetAll();
                    MessageBox.Show("Sua thanh cong");
                }
            }
            addTableView.Close();
        }

        [RelayCommand]
        private void DeleteTable(Table table)
        {
            Console.WriteLine("Delete Table");
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            db.Tables.Remove(table);
            db.SaveChanges();
            db.Entry(table).State = EntityState.Detached;
            _ = GetAll();
            MessageBox.Show("Delete Completed");

        }

        private void UpdateTable()
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (TableTmp.Contain == null)
                {
                    Exception exception = new Exception("Contain is emty");
                    throw exception;
                }
                db.Tables.Update(TableTmp);
                db.SaveChanges();
                db.Entry(TableTmp).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveNewTable()
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
                TableTmp.Id = db.Tables.IsNullOrEmpty() ? 1 : db.Tables.OrderBy(e => e.Id).Last().Id + 1;

                if (TableTmp.Contain.ToString() == "")
                {
                    Exception exception = new Exception("Contain is emty");
                    throw exception;
                }
                db.Tables.Add(TableTmp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
