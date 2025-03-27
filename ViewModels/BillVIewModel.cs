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


        [ObservableProperty] private ObservableCollection<Employee> employeeOb;
        [ObservableProperty] private ObservableCollection<OrderBill> orderBillOb;
        [ObservableProperty] private ObservableCollection<Ingredient> ingredientOb;



        [ObservableProperty] private int currentTabIndex;


        //[ObservableProperty] private List<SalaryBill> salaryBillOb;
        [ObservableProperty] private Visibility isLoading;
       



        [ObservableProperty] private string? nameSearch = "";


        [ObservableProperty] private Employee? selectedEmployee;
        [ObservableProperty] private SalaryBill? selectedSalary;
        [ObservableProperty] private OrderBill? selectedOrderBill;
        [ObservableProperty] private IngredientBill? selectedIngredientBill;




        [ObservableProperty] private string? fullName;
        [ObservableProperty] private string? employeeId;
        [ObservableProperty] private string? hours;
        [ObservableProperty] private string? total;
        [ObservableProperty] private string? role;
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


        private void SearchByName(string name)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            EmployeeOb.Clear();
            foreach (Employee employee in db.Employees.Include(e => e.Information).ToList())
            {
                if (employee.Information.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    EmployeeOb.Add(employee);
                }
            }
        }


        // Tìm kiếm hóa đơn theo ID
        private void SearchById(string id)
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
            IngredientOb.Clear();

            foreach (Ingredient ingredient in db.Ingredients.ToList()) // Loại bỏ .Include() không cần thiết
            {
                if (ingredient.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    IngredientOb.Add(ingredient);
                }
            }
        }



        partial void OnNameSearchChanged(string? value)
        {
         
            switch (CurrentTabIndex)
            {
                case 0: // Tab Nhân viên (Employee)
                    SearchByName(value);
                    break;
                case 1: // Tab Hóa đơn đặt hàng (Order Bill)
                    SearchById(value);
                    break;
                default:
                    SearchByIngreName(value);
                    break;
            }
        }

        public BillViewModel()
        {
            EmployeeOb = new ObservableCollection<Employee>();
            OrderBillOb = new ObservableCollection<OrderBill>();
            IngredientOb = new ObservableCollection<Ingredient>();

            //SalaryBillOb = new List<SalaryBill>();
            _ = Loading();
           

        }

        private async Task Loading()
        {
            IsLoading = Visibility.Visible;

            await GetAllEmployee();
            await GetAllOrderBill();
            await GetAllIngredient();
            IsLoading = Visibility.Hidden;
        }


        [RelayCommand]
        private async Task GetAllEmployee()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(1000);
            EmployeeOb.Clear();
            foreach (Employee employee in db.Employees.OrderByDescending(e => e.Id).Include(e => e.Information).Include(e => e.SalaryBills).ToList())
            {
                EmployeeOb.Add(employee);
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
        private async Task GetAllIngredient()
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            await Task.Delay(1000);
            IngredientOb.Clear();
            foreach (Ingredient ingredient in db.Ingredients.OrderByDescending(e => e.Id).ToList())
            {
                IngredientOb.Add(ingredient);
            }

        }



        [RelayCommand]
        private void Invoice(Employee selectedEmployee)
        {
            FullName = selectedEmployee.Information.Name;
            EmployeeId = selectedEmployee.Id.ToString();

            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            
            SelectedSalary = db.SalaryBills
                .Where(s => s.EmployeeId == selectedEmployee.Id)
                .OrderByDescending(s => s.Time)
                .FirstOrDefault();

            Hours = SelectedSalary?.TotalShifts.ToString();
            Role = selectedEmployee.Role.ToString();
            total = selectedEmployee.Salary.ToString();

            invoiceView = new SalaryBillView();
            invoiceView.DataContext = this;
            invoiceView.ShowDialog();
        }



        [RelayCommand]
        private void InvoiceOrder(OrderBill selectedOrderBill)
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
        private void InvoiceIngredient(int ingredientId)
        {
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;

            // Tìm hóa đơn nguyên liệu dựa trên IngredientId
            SelectedIngredientBill = db.IngredientBills
                .Where(ib => ib.IngredientId == ingredientId)
                .OrderByDescending(ib => ib.Time)
                .FirstOrDefault();

            if (SelectedIngredientBill == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn nguyên liệu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Gán dữ liệu cho các thuộc tính hiển thị
            IngreID = SelectedIngredientBill.IngredientId.ToString();
            NameIngre = db.Ingredients.FirstOrDefault(i => i.Id == ingredientId)?.Name ?? "N/A";
            Quantity = SelectedIngredientBill.Quantity.ToString();
            Price = SelectedIngredientBill.Price.ToString();
            TimeIngre = SelectedIngredientBill.Time.ToString();
            TotalIngre = SelectedIngredientBill.TotalPrice.ToString();


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


        //[RelayCommand]
        //private void Cancel()
        //{
        //    if (invoiceView != null && OrderBillView != null)
        //    {
        //        invoiceView.Close();
        //        OrderBillView.Close();
        //    }
        //}
    }
}
