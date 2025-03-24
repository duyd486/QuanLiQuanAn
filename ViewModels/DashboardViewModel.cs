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
using LiveCharts.Wpf;

namespace QuanLiQuanAn.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        public DashboardViewModel()
        {
            //Line
            Values1 = new ChartValues<double> { 3, 4, 6, 3, 2, 6 };

            double test = 12;

            Values1.Add(test);

            Values2 = new ChartValues<double> { 5, 3, 5, 7, 3, 9 };

            //Col
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });
            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };

            //Pie
            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }


        //Pie
        public Func<ChartPoint, string> PointLabel { get; set; }


        //Column
        ColumnSeries columnSeries = new ColumnSeries();
        public string[] Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        //Line
        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }
    }
}
