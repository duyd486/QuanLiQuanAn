﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using QuanLiQuanAn.Views;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuanLiQuanAn.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> userOb;

        public ObservableCollection<string> SortItems { get; } =
        [
        "All", "Male", "Female"
        ];

        [ObservableProperty]
        private Visibility isLoading;

        public IEnumerable<string> Gender { get; } =
        [
            "Male" , "Female"
        ];
        public IEnumerable<string> City { get; } =
        [
            "An Giang", "Ba Ria - Vung Tau", "Bac Lieu", "Bac Giang", "Bac Kan", "Bac Ninh",
            "Ben Tre", "Binh Duong", "Binh Dinh", "Binh Phuoc", "Binh Thuan", "Ca Mau",
            "Cao Bang", "Can Tho", "Da Nang", "Dak Lak", "Dak Nong", "Dien Bien", "Dong Nai",
            "Dong Thap", "Gia Lai", "Ha Giang", "Ha Nam", "Ha Noi", "Ha Tinh", "Hai Duong",
            "Hai Phong", "Hau Giang", "Hoa Binh", "Ho Chi Minh", "Hung Yen", "Khanh Hoa",
            "Kien Giang", "Kon Tum", "Lai Chau", "Lang Son", "Lao Cai", "Lam Dong",
            "Long An", "Nam Dinh", "Nghe An", "Ninh Binh", "Ninh Thuan", "Phu Tho",
            "Phu Yen", "Quang Binh", "Quang Nam", "Quang Ngai", "Quang Ninh", "Quang Tri",
            "Soc Trang", "Son La", "Tay Ninh", "Thai Binh", "Thai Nguyen", "Thanh Hoa",
            "Thua Thien Hue", "Tien Giang", "Tra Vinh", "Tuyen Quang", "Vinh Long",
            "Vinh Phuc", "Yen Bai"
        ];

        [ObservableProperty]
        private string? selectedSortItem;

        [ObservableProperty]
        private string? nameSearch = "";
        partial void OnNameSearchChanged(string? value)
        {
            SearchByName(value);
        }

        public UserViewModel()
        {
            UserOb = new ObservableCollection<User>();
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
            UserOb.Clear();
            foreach (User user in db.Users.OrderByDescending(e => e.Id).Include(e => e.Information).ToList())
            {
                UserOb.Add(user);
            }
        }
        [RelayCommand]
        private void SortGender(string gender)
        {
            SelectedSortItem = gender;
            switch (gender)
            {
                case "All":
                    _ = GetAll();
                    return;
                case "Male":

                    break;
                case "Female":

                    break;
            }
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            UserOb.Clear();
            foreach (User user in db.Users.Include(e => e.Information).ToList())
            {
                if (user.Information.Gender == gender)
                {
                    UserOb.Add(user);
                }
            }
        }

        [ObservableProperty]
        User? userTmp;
        [ObservableProperty]
        Information informationTmp;
        [ObservableProperty]
        private CustomerInfoView customerInfoView;
        [RelayCommand]
        private void InteractEmployee(User user)
        {
            UserTmp = new();
            UserTmp = user;
            InformationTmp = new();
            InformationTmp = user.Information;

            customerInfoView = new();
            customerInfoView.DataContext = this;
            customerInfoView.ShowDialog();
        }
        private void SearchByName(string name)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            UserOb.Clear();
            foreach (User user in db.Users.Include(e => e.Information).ToList())
            {
                if (user.Information.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    UserOb.Add(user);
                }
            }
        }

    }
}
