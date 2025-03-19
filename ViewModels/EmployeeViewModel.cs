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

namespace QuanLiQuanAn.ViewModels
{
    public partial class EmployeeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Employee> employeeOb;

        [ObservableProperty]
        private List<SalaryBill> salaryBillOb;

        public ObservableCollection<string> SortItems { get; } =
        [
        "All", "Waitress", "Cashier", "Chef"
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


        public EmployeeViewModel()
        {
            EmployeeOb = new ObservableCollection<Employee>();
            SalaryBillOb = new List<SalaryBill>();
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
            EmployeeOb.Clear();
            foreach (Employee employee in db.Employees.OrderByDescending(e => e.Id).Include(e => e.Information).Include(e => e.SalaryBills).ToList())
            {
                EmployeeOb.Add(employee);
            }

        }
        [RelayCommand]
        private void SortRole(string roleStr)
        {
            SelectedSortItem = roleStr;
            int role = 0;
            switch (roleStr)
            {
                case "All":
                    _ = GetAll();
                    return;
                case "Waitress":
                    role = 1;
                    break;
                case "Cashier":
                    role = 2;
                    break;
                case "Chef":
                    role = 3;
                    break;
            }
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            EmployeeOb.Clear();
            foreach (Employee employee in db.Employees.Include(e => e.Information).ToList())
            {
                if (employee.Role == role)
                {
                    EmployeeOb.Add(employee);
                }
            }
        }


        [ObservableProperty]
        Employee? employeeTmp;
        [ObservableProperty]
        Information informationTmp;
        [ObservableProperty]
        private DateOnly date = DateOnly.FromDateTime(DateTime.Today);
        private Views.AddEmployee addEmployeeView;
        private bool isEdit;
        [RelayCommand]
        private void InteractEmployee(Employee employee)
        {
            isEdit = true;
            EmployeeTmp = new();
            EmployeeTmp = employee;
            InformationTmp = new();
            InformationTmp = employee.Information;

            addEmployeeView = new();
            addEmployeeView.DataContext = this;
            addEmployeeView.ShowDialog();
        }

        [RelayCommand]
        private void AddEmployee()
        {
            isEdit = false;
            InformationTmp = new();
            EmployeeTmp = new();
            addEmployeeView = new();
            addEmployeeView.DataContext = this;
            addEmployeeView.ShowDialog();
        }
        [RelayCommand] 
        private void ApplyEmployee(string sender)
        {
            if (sender == "Apply")
            {
                if (!isEdit)
                {
                    SaveNewEmployee();
                }
                else
                {
                    UpdateEmployee();
                }
            }
            _ = GetAll();
            addEmployeeView.Close();
        }

        [RelayCommand]
        private void DeleteEmployee(Employee employee)
        {
            EmployeeTmp = new();
            EmployeeTmp = employee;
            QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
            SalaryBillOb.Clear();
            SalaryBillOb = (List<SalaryBill>)EmployeeTmp.SalaryBills;
            foreach (SalaryBill salaryBill in SalaryBillOb)
            {
                Console.WriteLine(salaryBill.EmployeeId);
            }
            Console.WriteLine("Delete" + EmployeeTmp.Id);

        }

        [RelayCommand]
        private void ImportExcel()
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }
            if(filePath == "")
            {
                return;
            }
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();

                var package = new ExcelPackage(new FileInfo(filePath));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    SalaryBill salaryBill = new SalaryBill();
                    salaryBill.Id =db.SalaryBills.IsNullOrEmpty() ? 1 : db.SalaryBills.OrderBy(e => e.Id).Last().Id + 1;

                    int employeeIdCollum = 1;
                    salaryBill.EmployeeId = Convert.ToInt32(worksheet.Cells[i, employeeIdCollum].Value);
                    
                    int totalShiftCollum = 3;
                    salaryBill.TotalShifts = Convert.ToInt32(worksheet.Cells[i, totalShiftCollum].Value);

                    salaryBill.Status = 1;

                    int timeCollum = 7;
                    string? timeStr = worksheet.Cells[i, timeCollum].Value.ToString();
                    salaryBill.Time = DateOnly.Parse(timeStr.Substring(0, timeStr.IndexOf(" ")));

                    db.SalaryBills.Add(salaryBill);
                    db.SaveChanges();
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }



        private void UpdateEmployee()
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (EmployeeTmp.Salary.ToString() == "")
                {
                    Exception exception = new Exception("Salary was write in wrong condiction");
                    throw exception;
                }
                else if (InformationTmp.CitizenId.ToString() == "")
                {
                    Exception exception = new Exception("Cityzen Id was write in wrong condiction");
                    throw exception;
                }
                else if (InformationTmp.Name == "")
                {
                    Exception exception = new Exception("Name is emty");
                    throw exception;
                }

                db.Informations.Update(InformationTmp);
                db.Employees.Update(EmployeeTmp);
                db.SaveChanges();
                db.Entry(InformationTmp).State = EntityState.Detached;

                MessageBox.Show("Sua thanh cong");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SaveNewEmployee()
        {
            try
            {
                QuanannhatContext db = Singleton.DatabaseSingleton.GetInstance().db;
                InformationTmp.Id = db.Informations.IsNullOrEmpty() ? 1 : db.Informations.OrderBy(e => e.Id).Last().Id + 1;
                EmployeeTmp.Id = db.Employees.IsNullOrEmpty() ? 1 : db.Employees.OrderBy(e => e.Id).Last().Id + 1;
                EmployeeTmp.InformationId = InformationTmp.Id;
                InformationTmp.Birth = Date;

                if (EmployeeTmp.Salary.ToString() == "")
                {
                    Exception exception = new Exception("Salary was write in wrong condiction");
                    throw exception;
                }
                else if (InformationTmp.CitizenId.ToString() == "")
                {
                    Exception exception = new Exception("Cityzen Id was write in wrong condiction");
                    throw exception;
                }
                else if (InformationTmp.Name == "")
                {
                    Exception exception = new Exception("Name is emty");
                    throw exception;
                }

                db.Informations.Add(InformationTmp);
                db.Employees.Add(EmployeeTmp);
                db.SaveChanges();
                MessageBox.Show("Them thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

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

    }
}
