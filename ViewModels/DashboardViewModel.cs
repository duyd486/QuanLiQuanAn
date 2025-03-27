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
using System.Windows.Media;
using System.Windows.Xps.Packaging;

namespace QuanLiQuanAn.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {

        private DateOnly date = DateOnly.FromDateTime(DateTime.Today);
        [ObservableProperty] double salaryOut = 0;
        [ObservableProperty] double ingredientOut = 0;
        [ObservableProperty] double totalUser = 0;
        [ObservableProperty] double totalBadRate = 0;

        [ObservableProperty] private ObservableCollection<Rate> goodRateOb;
        [ObservableProperty] private ObservableCollection<Rate> badRateOb;
        double totalEmploy = 0;
        QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
        [ObservableProperty] private string salaryOutcome;
        [ObservableProperty] private string ingredientOutcome;
        [ObservableProperty] private string totalEmployee;
        [ObservableProperty] private string totalOutcome;

        //Col
        public SeriesCollection ColSeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        //Pie
        public Func<ChartPoint, string> PiePointLabel { get; set; }
        public SeriesCollection PieSeriesCollection { get; set; }

        public Func<ChartPoint, string> AgePiePointLabel { get; set; }
        public SeriesCollection AgePieSeriesCollection { get; set; }
        //Line
        public ChartValues<double> SalaryLineChartValue { get; set; }
        public ChartValues<double> IngredientLineChartValue { get; set; }


        public DashboardViewModel()
        {
            InitLastMonthData();
            InitPieChartData();
            InitLineChartData();
            InitColChartData();
            InitRateData();
        }


        private void InitRateData()
        {
            GoodRateOb = new ObservableCollection<Rate>();
            BadRateOb = new ObservableCollection<Rate>();
            foreach (Rate rate in db.Rates.Include(e => e.User).ThenInclude(e => e.Information))
            {
                if(rate.Type == 3)
                {
                    GoodRateOb.Add(rate);
                }
                else
                {
                    BadRateOb.Add(rate);
                }
            }
        }
        private void InitLastMonthData()
        {
            SalaryOut = 0;
            IngredientOut = 1;
            totalEmploy = 0;
            totalUser = 0;
            totalBadRate = 0;
            foreach(SalaryBill salaryBill in db.SalaryBills.Include(e => e.Employee).ToList())
            {
                if(salaryBill.Time.Value.Month == date.Month && salaryBill.Time.Value.Year == date.Year)
                {
                    SalaryOut += (double)(salaryBill.TotalShifts * salaryBill.Employee.Salary);
                }
            }
            SalaryOutcome = SalaryOut.ToString();
            foreach(IngredientBill ingredientBill in db.IngredientBills)
            {
                if(ingredientBill.Time.Value.Month == date.Month && ingredientBill.Time.Value.Year == date.Year)
                {
                    IngredientOut += (int)ingredientBill.TotalPrice;
                }
            }
            IngredientOutcome = IngredientOut.ToString();
            foreach(Employee employee in db.Employees.Where(e => e.Status == 2))
            {
                totalEmploy++;
            }
            TotalEmployee = totalEmploy.ToString();
            TotalOutcome = (SalaryOut + IngredientOut).ToString();
            foreach(User user in db.Users)
            {
                totalUser++;
            }
            foreach(Rate rate in db.Rates)
            {
                if(rate.Type == 1)
                {
                    totalBadRate++;
                }
            }

        }

        private void InitPieChartData()
        {
            PiePointLabel = chartPoint =>
            string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            PieSeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Salary",
                    Values = new ChartValues<double> { SalaryOut },
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B23D23"))
                },
                new PieSeries
                {
                    Title = "Ingredient",
                    Values = new ChartValues<double> { IngredientOut },
                    DataLabels = true,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C0BEEE"))
                },
            };

            AgePiePointLabel = chartPoint =>
            string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            List<int> ageList = new List<int>();
            AgePieSeriesCollection = new SeriesCollection();
            foreach (User user in db.Users.Include(e => e.Information))
            {
                int age = date.Year - user.Information.Birth.Value.Year;
                if (!ageList.Contains(age))
                {
                    ageList.Add(age);
                }
            }
            foreach(int i in ageList)
            {
                double count = 0;
                foreach(User user in db.Users.Include(e => e.Information))
                {
                    if(date.Year - user.Information.Birth.Value.Year == i)
                    {
                        count++;
                    }
                }
                // Create Pie Series with title = i; Value = count
                AgePieSeriesCollection.Add(new PieSeries
                {
                    Title = i.ToString(),
                    Values = new ChartValues<double> { count },
                    DataLabels = true
                });
            }



        }
        private void InitLineChartData()
        {
            SalaryLineChartValue = new ChartValues<double> { 10 };
            IngredientLineChartValue = new ChartValues<double> { 10 };
            double salaryOutByMonth = 0;
            double ingredientOutByMonth = 0;

            for(int i = 1; i <= date.Month; i++)
            {
                salaryOutByMonth = 0;
                foreach (SalaryBill salaryBill in db.SalaryBills.Include(e => e.Employee).ToList())
                {
                    if (salaryBill.Time.Value.Month == i && salaryBill.Time.Value.Year == date.Year)
                    {
                        salaryOutByMonth += (double)(salaryBill.TotalShifts * salaryBill.Employee.Salary);
                    }
                }
                SalaryLineChartValue.Add(salaryOutByMonth);
            }
            for (int i = 1; i <= date.Month; i++)
            {
                ingredientOutByMonth = 0;
                foreach (IngredientBill ingredientBill in db.IngredientBills)
                {
                    if (ingredientBill.Time.Value.Month == i && ingredientBill.Time.Value.Year == date.Year)
                    {
                        ingredientOutByMonth += (int)ingredientBill.TotalPrice;
                    }
                }
                IngredientLineChartValue.Add(ingredientOutByMonth);
            }
        }
        private void InitColChartData()
        {
            ColSeriesCollection = new SeriesCollection();

            foreach (Dish dish in db.Dishes.Include(e => e.Orders).ThenInclude(e => e.Orderbill).ToList())
            {
                double totalSale = 0;
                foreach (Order order in dish.Orders)
                {
                    if(order.Orderbill.Time.Month == date.Month && order.Orderbill.Time.Year == date.Year && order.Orderbill.BillStatus == 3)
                    {
                        totalSale += (double)order.TotalPrice;
                    }
                }
                ColSeriesCollection.Add(new RowSeries
                {
                    Title = dish.Name,
                    Values = new ChartValues<double> { totalSale }
                });
            }
            Labels = [date.Year.ToString()];
            Formatter = value => value.ToString("N");
        }
    }
}
