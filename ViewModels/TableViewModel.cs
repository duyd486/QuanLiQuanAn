﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using QuanLiQuanAn.DBContext;
using QuanLiQuanAn.Models;
using QuanLiQuanAn.Singleton;
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using QuanLiQuanAn.Views;
using System.Windows.Input;
using QuanLiQuanAn.Views.Modals;

namespace QuanLiQuanAn.ViewModels
{
    public partial class TableViewModel : ObservableObject
    {
        private ModalTableView modalTableView;
        private bool isEdit;

        [ObservableProperty] private ObservableCollection<Table> tableOb;
        [ObservableProperty] private string? selectedSortItem;
        [ObservableProperty] private Visibility isLoading;
        [ObservableProperty] Table? tableTmp;

        public ObservableCollection<string> SortItems { get; } =
        [
        "All", "Table For 2", "Table For 3", "Table For 4", "Above 4"
        ];
        public IEnumerable<string> Gender { get; } =
        [
            "Male" , "Female"
        ];

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
            QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
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
            QuanannhatContext db = DatabaseSingleton.GetInstance().db;
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

        [RelayCommand]
        private void InteractTable(Table table)
        {
            isEdit = true;

            TableTmp = new();
            TableTmp = table;

            modalTableView = new();
            modalTableView.DataContext = this;
            modalTableView.ShowDialog();
        }

        [RelayCommand]
        private void AddTable()
        {
            isEdit = false;

            TableTmp = new();

            modalTableView = new();
            modalTableView.DataContext = this;
            modalTableView.ShowDialog();
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
                }
                else
                {
                    UpdateTable();
                    _ = GetAll();
                }
            }
            modalTableView.Close();
        }

        [RelayCommand]
        private void DeleteTable(Table table)
        {
            QuanannhatContext db = DatabaseSingleton.GetInstance().db;

            db.Tables.Remove(table);
            db.SaveChanges();
            db.Entry(table).State = EntityState.Detached;

            _ = GetAll();
            MessageBox.Show("Delete Completed!");
        }

        private void UpdateTable()
        { 
            try
            {
                QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (TableTmp.Contain == null)
                {
                    Exception exception = new Exception("Contain is emty!");
                    throw exception;
                }
                db.Tables.Update(TableTmp);
                db.SaveChanges();
                db.Entry(TableTmp).State = EntityState.Detached;
                MessageBox.Show("Edit Completed!");
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
                QuanannhatContext db = DatabaseSingleton.GetInstance().db;
                TableTmp.Id = db.Tables.IsNullOrEmpty() ? 1 : db.Tables.OrderBy(e => e.Id).Last().Id + 1;

                if (TableTmp.Contain == null)
                {
                    Exception exception = new Exception("Contain is emty");
                    throw exception;
                }
                db.Tables.Add(TableTmp);
                db.SaveChanges();
                db.Entry(TableTmp).State = EntityState.Detached;
                MessageBox.Show("Add Completed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
