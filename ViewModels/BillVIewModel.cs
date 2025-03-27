using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using QuanLiQuanAn.DBContext;
using QuanLiQuanAn.Models;
using QuanLiQuanAn.Views;
using QuanLiQuanAn.Views.Modals;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiQuanAn.ViewModels
{
    public partial class BillViewModel : ObservableObject
    {


        //[ObservableProperty] private ObservableCollection<Employee> employeeOb;
        [ObservableProperty] private ObservableCollection<OrderBill> orderBillOb;
        //[ObservableProperty] private ObservableCollection<Ingredient> ingredientOb;
        [ObservableProperty] private ObservableCollection<SalaryBill> salaryBillOb;
        [ObservableProperty] private ObservableCollection<IngredientBill> ingredientBillOb;


        [ObservableProperty] private int currentTabIndex;


        [ObservableProperty] private Visibility isLoading;
       



        [ObservableProperty] private string? nameSearch = "";


        //[ObservableProperty] private Employee? selectedEmployee;
        [ObservableProperty] private SalaryBill? selectedSalary;
        [ObservableProperty] private OrderBill? selectedOrderBill;
        [ObservableProperty] private IngredientBill? selectedIngredientBill;




        [ObservableProperty] private string? fullName;
        [ObservableProperty] private string? employeeId;
        [ObservableProperty] private int? totalShifts;
        [ObservableProperty] private string? totalOne;
        [ObservableProperty] private string? role;
        [ObservableProperty] private int? salaryPerShift;
        [ObservableProperty] private int? totalShiftsCount;
        //[ObservableProperty] private string? totalPriceee;



        private SalaryBillView invoiceView;
        private bool IsEnabled;



        [ObservableProperty] private string? user;
        [ObservableProperty] private string? table;
        [ObservableProperty] private string? discount;
        [ObservableProperty] private int? order;
        [ObservableProperty] private int? bill;
        [ObservableProperty] private string? time;
        [ObservableProperty] private string? totalprice;
        private OrderBillView OrderBillView;



        [ObservableProperty] private string? ingreID;
        [ObservableProperty] private string? nameIngre;
        [ObservableProperty] private string? quantity;
        [ObservableProperty] private string? price;
        [ObservableProperty] private string? timeIngre;
        [ObservableProperty] private string? totalIngre;
        private IngredientBillView IngredientBillView;


        //private void SearchByName(string name)
        //{
        //    QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
        //    EmployeeOb.Clear();
        //    foreach (Employee employee in db.Employees.Include(e => e.Information).ToList())
        //    {
        //        if (employee.Information.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            EmployeeOb.Add(employee);
        //        }
        //    }
        //}


        private void SearchByIdSalary(string id)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            SalaryBillOb.Clear();
            foreach (SalaryBill salaryBill in db.SalaryBills.OrderBy(o => o.Id).ToList())
            {
                if (salaryBill.Id.ToString().Contains(id, StringComparison.InvariantCultureIgnoreCase))
                {
                    SalaryBillOb.Add(salaryBill);  
                }
            }
        }



      
        private void SearchByIdOrder(string id)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            OrderBillOb.Clear();
            foreach (OrderBill orderBill in db.OrderBills.OrderBy(o => o.Id).ToList())
            {
                if (orderBill.Id.ToString().Contains(id, StringComparison.InvariantCultureIgnoreCase))
                {
                    OrderBillOb.Add(orderBill);
                }
            }
        }

        // Tìm kiếm hóa đơn Ingredinent theo Name
        private void SearchByIngreName(string name)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            IngredientBillOb.Clear();
            foreach (IngredientBill ingredientBill in db.IngredientBills.ToList()) 
            {
                if (ingredientBill.Ingredient.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    IngredientBillOb.Add(ingredientBill);
                }
            }
        }



        partial void OnNameSearchChanged(string? value)
        {
         
            switch (CurrentTabIndex)
            {
                case 0: // Tab Nhân viên (Employee)
                    SearchByIdSalary(value);
                    break;
                case 1: // Tab Hóa đơn đặt hàng (Order Bill)
                    SearchByIdOrder(value);
                    break;
                default:
                    SearchByIngreName(value);
                    break;
            }
        }

        public BillViewModel()
        {      
            SalaryBillOb = new ObservableCollection<SalaryBill>();
            OrderBillOb = new ObservableCollection<OrderBill>();
            IngredientBillOb = new ObservableCollection<IngredientBill>();    
            _ = Loading();
        }

        private async Task Loading()
        {
            IsLoading = Visibility.Visible;
            await GetAllSalaryBill();
            await GetAllOrderBill();
            await GetAllIngredientBill();
            IsLoading = Visibility.Hidden;
        }


        [RelayCommand]
        private async Task GetAllSalaryBill()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(1000);
            SalaryBillOb.Clear();

            foreach (SalaryBill salaryBill in db.SalaryBills
                .Include(s => s.Employee)
                .ThenInclude(e => e.Information)
                .OrderByDescending(e => e.Id)
                .ToList())
            {
                SalaryBillOb.Add(salaryBill);
            }
        }


        [RelayCommand]
        private async Task GetAllOrderBill()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(1000);
            OrderBillOb.Clear();
            foreach (OrderBill orderBill in db.OrderBills.OrderByDescending(e => e.Id).ToList())
            {
               OrderBillOb.Add(orderBill);
            }
        }

        [RelayCommand]
        private async Task GetAllIngredientBill()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(1000);
            IngredientBillOb.Clear();
            foreach (IngredientBill ingredientBill in db.IngredientBills
                .Include(s => s.Ingredient)
                .OrderByDescending(e => e.Id)
                .ToList())
            {
                IngredientBillOb.Add(ingredientBill);
            }

        }



        [RelayCommand]
        private void InvoiceSalaryBill(SalaryBill selectedSalaryBill)
        {
            if (selectedSalaryBill == null || selectedSalaryBill.Employee == null) return;

            // Gán thông tin từ SalaryBill được chọn
            FullName = selectedSalaryBill.Employee.Information?.Name ?? "N/A";
            EmployeeId = selectedSalaryBill.Employee.Id.ToString();
            Role = selectedSalaryBill.Employee.Role.ToString();
            SalaryPerShift = selectedSalaryBill.Employee.Salary;
            TotalShifts = selectedSalaryBill.TotalShifts;
            Time = selectedSalaryBill.Time?.ToString("dd/MM/yyyy");

            // Mở cửa sổ hóa đơn
            invoiceView = new SalaryBillView();
            invoiceView.DataContext = this;
            invoiceView.ShowDialog();
        }

        [RelayCommand]
        private void InvoiceOrderBill(OrderBill selectedOrderBill)
        {
       
            User = selectedOrderBill.UserId.ToString();
            Table = selectedOrderBill.TableId.ToString();
            Discount = selectedOrderBill.DiscountId.ToString();
            Order = selectedOrderBill.OrderStatus;
            Bill = selectedOrderBill.BillStatus;
            Time = selectedOrderBill.Time.ToString();
            Totalprice = selectedOrderBill.TotalPrice.ToString();

            OrderBillView = new OrderBillView();
            OrderBillView.DataContext = this;
            OrderBillView.ShowDialog();
        }


        [RelayCommand]
        private void InvoiceIngredientBill(IngredientBill selectedIngredientBill)
        {
          
            //// Gán dữ liệu cho các thuộc tính hiển thị
            IngreID = selectedIngredientBill.IngredientId.ToString();
            NameIngre = selectedIngredientBill.Ingredient?.Name ?? "N/A";
            Quantity = selectedIngredientBill.Quantity.ToString();
            Price = selectedIngredientBill.Price.ToString();
            TimeIngre = selectedIngredientBill.Time.ToString();
            TotalIngre = selectedIngredientBill.TotalPrice.ToString();


            // Hiển thị cửa sổ chi tiết hóa đơn nguyên liệu
            IngredientBillView = new IngredientBillView();
            IngredientBillView.DataContext = this;
            IngredientBillView.ShowDialog();
        }


        [RelayCommand]
        private void Printed()
        {
            if (invoiceView == null && OrderBillView == null && IngredientBillView == null)
            {
                MessageBox.Show("Vui lòng kiểm tra lại !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    if (invoiceView != null)
                    {
                        printDialog.PrintVisual(invoiceView, "Salary Bill");
                    }

                    if (OrderBillView != null)
                    {
                        printDialog.PrintVisual(OrderBillView, "Order Bill");
                    }

                    if (IngredientBillView != null)
                    {
                        printDialog.PrintVisual(IngredientBillView, "Ingredient Bill");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while printing: {ex.Message}", "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsEnabled = true;
            }
        }
    }
}
