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
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using LiveCharts;
using LiveCharts.Defaults;

namespace QuanLiQuanAn.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        public DashboardViewModel()
        {
            Values1 = new ChartValues<double> { 3, 4, 6, 3, 2, 6 };

            double test = 12;

            Values1.Add(test);

            Values2 = new ChartValues<double> { 5, 3, 5, 7, 3, 9 };
        }


        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }
    }
}
