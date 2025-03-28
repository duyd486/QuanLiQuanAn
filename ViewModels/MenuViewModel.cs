using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using QuanLiQuanAn.DBContext;
using QuanLiQuanAn.Models;
using QuanLiQuanAn.Views;
using QuanLiQuanAn.Views.Modals;
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Windows.Input;

namespace QuanLiQuanAn.ViewModels
{
    public partial class MenuViewModel : ObservableObject
    {
        private bool isEdit;
        private int timeDelay = 800;
        private ModalDishView modalDishView;
        private ModalDishlistView modalDishlistView;

        [ObservableProperty] private ObservableCollection<Dish> dishOb;
        [ObservableProperty] private ObservableCollection<Dishlist> dishlistOb;
        [ObservableProperty] private Visibility isLoading;
        [ObservableProperty] private string? selectedSortItem;
        [ObservableProperty] private string selectedDishlist;
        [ObservableProperty] private string? nameSearch = "";
        [ObservableProperty] Dish? dishTmp;
        [ObservableProperty] Dishlist dishlistTmp;

        public ObservableCollection<string> DishlistsStr { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> SortItems { get; } =
        [
        "All"
        ];

        partial void OnNameSearchChanged(string? value)
        {
            SearchByName(value);
        }


        public MenuViewModel()
        {
            DishOb = new ObservableCollection<Dish>();
            DishlistOb = new ObservableCollection<Dishlist>();
            _ = Loading();
            SelectedSortItem = "All";
        }


        private async Task Loading()
        {
            IsLoading = Visibility.Visible;

            await GetAllDish();

            await GetAllDishlist();

            IsLoading = Visibility.Hidden;

        }

        private async Task GetAllDish()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(timeDelay);
            DishOb.Clear();
            foreach (Dish dish in db.Dishes.OrderByDescending(e => e.Id))
            {
                DishOb.Add(dish);
            }
        }
        private async Task GetAllDishlist()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            await Task.Delay(100);
            DishlistOb.Clear();
            DishlistsStr.Clear();
            SortItems.Clear();
            SortItems.Add("All");
            foreach (Dishlist dishlist in db.Dishlists.OrderByDescending(e => e.Id))
            {
                DishlistOb.Add(dishlist);
                SortItems.Add(dishlist.Name);
                DishlistsStr.Add(dishlist.Name);
            }
            SelectedSortItem = "All";
        }


        [RelayCommand]
        private void SortDish(string dishStr)
        {
            SelectedSortItem = dishStr;
            if (dishStr == "All")
            {
                _ = Loading();
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


        [RelayCommand]
        private void InteractDishlist(string name)
        {
            isEdit = true;
            if (name == "All")
            {
                MessageBox.Show("Cant Interact With All!!!");
                return;
            }
            DishlistTmp = new();
            foreach(Dishlist dishlist in Singleton.DatabaseSingleton.GetInstance().db.Dishlists.Where(e => e.Name == name))
            {
                DishlistTmp = dishlist;
            }
            modalDishlistView = new();
            modalDishlistView.DataContext = this;
            modalDishlistView.ShowDialog();
        }
        [RelayCommand]
        private void AddDishlist()
        {
            isEdit = false;
            DishlistTmp = new();

            modalDishlistView = new();
            modalDishlistView.DataContext = this;
            modalDishlistView.ShowDialog();
        }
        [RelayCommand]
        private void ApplyDishlist(string sender)
        {
            if (sender == "Apply")
            {
                if (!isEdit)
                {
                    SaveNew(DishlistTmp);
                    MessageBox.Show("Add completed!!!");
                    _ = GetAllDishlist();
                }
                else
                {
                    Update(DishlistTmp);
                    MessageBox.Show("Edit completed!!!");
                    _ = GetAllDishlist();
                }
            }
            modalDishlistView.Close();
        }
        [RelayCommand]
        private void DeleteDishlist(string name)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            if (name == "All")
            {
                MessageBox.Show("Cant Interact With All!!!");
                return;
            }
            DishlistTmp = new();
            foreach (Dishlist dishlist in db.Dishlists.Where(e => e.Name == name))
            {
                DishlistTmp = dishlist;
            }
            foreach(Dish dish in db.Dishes.Where(e => e.Dishlist == DishlistTmp))
            {
                dish.Dishlist = null;
            }
            db.Dishlists.Remove(DishlistTmp);
            db.SaveChanges();
            db.Entry(DishlistTmp).State = EntityState.Detached;
            MessageBox.Show("Delete Completed");
            _ = Loading();
            SelectedDishlist = "All";
        }

        [RelayCommand]
        private void InteractDish(Dish dish)
        {
            isEdit = true;
            SelectedDishlist = dish.Dishlist?.Name;
            DishTmp = new();
            DishTmp = dish;

            DishlistTmp = new();
            DishlistTmp = dish.Dishlist;

            modalDishView = new();
            modalDishView.DataContext = this;
            modalDishView.ShowDialog();
        }
        [RelayCommand]
        private void AddDish()
        {
            isEdit = false;
            DishTmp = new();

            modalDishView = new();
            modalDishView.DataContext = this;
            modalDishView.ShowDialog();
        }
        [RelayCommand]
        private void ApplyDish(string sender)
        {
            if (sender == "Apply")
            {
                if (!isEdit)
                {
                    SaveNew(DishTmp);
                }
                else
                {
                    Update(DishTmp);
                    MessageBox.Show("Sua thanh cong");
                }
            }
            _ = Loading();
            SortDish(SelectedSortItem);
            modalDishView.Close();
        }

        [RelayCommand]
        private void DeleteDish(Dish dish)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            db.Dishes.Remove(dish);
            db.SaveChanges();
            db.Entry(dish).State = EntityState.Detached;
            SortDish(SelectedSortItem);
            MessageBox.Show("Delete Completed");
        }

        private void Update(Dish dish)
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (DishTmp.Name.IsNullOrEmpty() || DishTmp.Price == null || DishTmp.Description.IsNullOrEmpty())
                {
                    Exception exception = new Exception("Please fill all the blank");
                    throw exception;
                }
                foreach (Dishlist dishlist in db.Dishlists.Where(x => x.Name == SelectedDishlist))
                {
                    DishTmp.Dishlist = dishlist;
                }
                db.Dishes.Update(DishTmp);
                db.SaveChanges();
                db.Entry(DishTmp).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Update(Dishlist dishlist)
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (DishlistTmp.Name.IsNullOrEmpty())
                {
                    Exception exception = new Exception("Please fill all the blank");
                    throw exception;
                }
                db.Dishlists.Update(DishlistTmp);
                db.SaveChanges();
                db.Entry(DishlistTmp).State = EntityState.Detached;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void SaveNew(Dish dish)
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
                DishTmp.Id = db.Dishes.IsNullOrEmpty() ? 1 : db.Dishes.OrderBy(e => e.Id).Last().Id + 1;

                if (DishTmp.Name.IsNullOrEmpty() || DishTmp.Price == null || DishTmp.Description.IsNullOrEmpty())
                {
                    Exception exception = new Exception("Please fill all the blank");
                    throw exception;
                }
                foreach (Dishlist dishlist in db.Dishlists.Where(x => x.Name == SelectedDishlist))
                {
                    DishTmp.Dishlist = dishlist;
                }
                db.Dishes.Add(DishTmp);
                db.SaveChanges();
                MessageBox.Show("Add Completed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveNew(Dishlist dishlist)
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
                DishlistTmp.Id = db.Dishlists.IsNullOrEmpty() ? 1 : db.Dishlists.OrderBy(e => e.Id).Last().Id + 1;

                if (DishlistTmp.Name.IsNullOrEmpty())
                {
                    Exception exception = new Exception("Please fill all the blank");
                    throw exception;
                }
                db.Dishlists.Add(DishlistTmp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchByName(string name)
        {
            timeDelay = 0;
            SortDish(SelectedSortItem);
            for (int i = 0; i < DishOb.Count; i++)
            {
                if (!DishOb[i].Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    DishOb.Remove(DishOb[i]);
                }
            }
            timeDelay = 800;
        }

    }
}
