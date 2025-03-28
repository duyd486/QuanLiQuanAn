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
using QuanLiQuanAn.Views.Modals;
using OfficeOpenXml.Style;
using QuanLiQuanAn.Singleton;
using System.Drawing;

namespace QuanLiQuanAn.ViewModels
{
    public partial class EmployeeViewModel : ObservableObject
    {
        private ModalEmployeeView modalEmployeeView;
        private bool isEdit;
        private int timeDelay = 800;

        [ObservableProperty] private ObservableCollection<Employee> employeeOb;
        [ObservableProperty] private List<SalaryBill> salaryBillOb;
        [ObservableProperty] Employee? employeeTmp;
        [ObservableProperty] Information informationTmp;
        [ObservableProperty] private DateOnly date = DateOnly.FromDateTime(DateTime.Today);
        [ObservableProperty] private Visibility isLoading;
        [ObservableProperty] private string? selectedSortItem;
        [ObservableProperty] private string? nameSearch = "";
        [ObservableProperty] private bool isActive = true;
        public ObservableCollection<string> SortItems { get; } =
        [
        "All", "Waitress", "Cashier", "Chef"
        ];
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

        partial void OnIsActiveChanged(bool value)
        {
            _ = GetAll();
        }
        partial void OnNameSearchChanged(string? value)
        {
            SearchByName(value);
        }

        public EmployeeViewModel()
        {
            DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            EmployeeOb = new ObservableCollection<Employee>();
            SalaryBillOb = new List<SalaryBill>();
            IsActive = true;
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
            await Task.Delay(timeDelay);
            EmployeeOb.Clear();
            foreach (Employee employee in db.Employees.OrderByDescending(e => e.Id).Include(e => e.Information).Include(e => e.SalaryBills).ToList())
            {
                if (IsActive)
                {
                    if(employee.Status == 2)
                    {
                        EmployeeOb.Add(employee);
                    }
                }
                else
                {
                    if(employee.Status == 1)
                    {
                        EmployeeOb.Add(employee);
                    }
                }
            }
            SelectedSortItem = "All";
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
            QuanannhatContext db = DatabaseSingleton.GetInstance().db;
            EmployeeOb.Clear();
            foreach (Employee employee in db.Employees.Include(e => e.Information).ToList())
            {
                if (employee.Role == role)
                {
                    if (IsActive)
                    {
                        if (employee.Status == 2)
                        {
                            EmployeeOb.Add(employee);
                        }
                    }
                    else
                    {
                        if (employee.Status == 1)
                        {
                            EmployeeOb.Add(employee);
                        }
                    }
                }
            }
        }

        [RelayCommand]
        private void InteractEmployee(Employee employee)
        {
            isEdit = true;

            EmployeeTmp = new();
            EmployeeTmp = employee;
            InformationTmp = new();
            InformationTmp = employee.Information;

            modalEmployeeView = new();
            modalEmployeeView.DataContext = this;
            modalEmployeeView.ShowDialog();
        }

        [RelayCommand]
        private void AddEmployee()
        {
            isEdit = false;
            InformationTmp = new();
            EmployeeTmp = new();

            modalEmployeeView = new();
            modalEmployeeView.DataContext = this;
            modalEmployeeView.ShowDialog();
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
            modalEmployeeView.Close();
        }

        [RelayCommand]
        private void DeleteEmployee(Employee employee)
        {
            QuanannhatContext db = DatabaseSingleton.GetInstance().db;
            employee.Status = 1;
            db.Update(employee);
            db.SaveChanges();
            _ = GetAll();
            
            MessageBox.Show("Unactive Employee Completed!");
        }

        [RelayCommand]
        private void PayBill(SalaryBill salaryBill)
        {
            QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
            salaryBill.Status = 2;
            db.SalaryBills.Update(salaryBill);
            db.SaveChanges();
            db.Entry(salaryBill).State = EntityState.Detached;
            MessageBox.Show("Pay Bill Completed");
        }

        [RelayCommand]
        private void ImportExcel()
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx";
            openFileDialog.Title = "Choose Your Salary Bills Excel File";

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }
            if(filePath == "")
            {
                return;
            }
            try
            {
                QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                var package = new ExcelPackage(new FileInfo(filePath));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    Console.WriteLine(i);
                    SalaryBill salaryBill = new SalaryBill();
                    salaryBill.Id = db.SalaryBills.IsNullOrEmpty() ? 1 : db.SalaryBills.OrderBy(e => e.Id).Last().Id + 1;

                    int employeeIdCollum = 1;
                    salaryBill.EmployeeId = Convert.ToInt32(worksheet.Cells[i, employeeIdCollum].Value);
                    
                    int totalShiftCollum = 3;
                    salaryBill.TotalShifts = Convert.ToInt32(worksheet.Cells[i, totalShiftCollum].Value);

                    salaryBill.Status = 1;

                    int timeCollum = 7;
                    string? timeStr = worksheet.Cells[i, timeCollum].Value.ToString();
                    salaryBill.Time = DateOnly.Parse(timeStr);

                    db.SalaryBills.Add(salaryBill);
                    db.SaveChanges();
                }
                MessageBox.Show("Import Excel Completed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            _ = Loading();
        }

        [RelayCommand]
        [STAThread]
        private void ExportExcel()
        {
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Salary");
                worksheet.Row(1).Height = 20;
                worksheet.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 30;
                worksheet.Cells["F2"].Formula = "C2*E2";

                string[] headers = { "ID", "Name", "Total Shifts", "Role", "Salary", "Total Salary", "Time" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(178, 61, 35));
                    worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(Color.FromArgb(245, 213, 194));
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Column(i + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                int row = 2;
                foreach (Employee employee in db.Employees.Where(e => e.Status == 2).Include(e => e.Information).ToList())
                {
                    worksheet.Cells[$"F{row}"].Formula = $"C{row}*E{row}";

                    var cell1 = worksheet.Cells[row, 1];
                    var cell2 = worksheet.Cells[row, 2];
                    var cell3 = worksheet.Cells[row, 4];
                    var cell4 = worksheet.Cells[row, 5];
                    var cell5 = worksheet.Cells[row, 7];

                    worksheet.Cells[row, 3].Style.Border.BorderAround(ExcelBorderStyle.Dotted);

                    cell1.Value = employee.Id;
                    cell2.Value = employee.Information.Name;
                    cell3.Value = employee.Role;
                    cell4.Value = employee.Salary;
                    cell5.Value = date.ToString();
                    row++;
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Choose File Excel Location";
                saveFileDialog.FileName = "Salary.xlsx";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                    MessageBox.Show($"File Location At: {filePath}");
                }
                else
                {
                    MessageBox.Show("Cancel Save file.");
                }
            }
        }


        private void UpdateEmployee()
        {
            try
            {
                QuanannhatContext db = DatabaseSingleton.GetInstance().db = new QuanannhatContext();
                if (EmployeeTmp.Information.Name.IsNullOrEmpty() || EmployeeTmp.Salary == null || EmployeeTmp.Information.Birth == null || EmployeeTmp.Information.Phone.IsNullOrEmpty() || EmployeeTmp.Information.CitizenId == null)
                {
                    Exception exception = new Exception("Please fill all the blank");
                    throw exception;
                }
                db.Informations.Update(InformationTmp);
                db.Employees.Update(EmployeeTmp);
                db.SaveChanges();
                db.Entry(InformationTmp).State = EntityState.Detached;
                db.Entry(EmployeeTmp).State = EntityState.Detached;

                MessageBox.Show("Edit Completed!");

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
                QuanannhatContext db = DatabaseSingleton.GetInstance().db;
                InformationTmp.Id = db.Informations.IsNullOrEmpty() ? 1 : db.Informations.OrderBy(e => e.Id).Last().Id + 1;
                EmployeeTmp.Id = db.Employees.IsNullOrEmpty() ? 1 : db.Employees.OrderBy(e => e.Id).Last().Id + 1;
                EmployeeTmp.InformationId = InformationTmp.Id;
                InformationTmp.Birth = Date;

                db.Informations.Add(InformationTmp);
                db.Employees.Add(EmployeeTmp);
                db.SaveChanges();
                db.Entry(InformationTmp).State = EntityState.Detached;
                db.Entry(EmployeeTmp).State = EntityState.Detached;
                MessageBox.Show("Add Completed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchByName(string name)
        {
            timeDelay = 0;
            SortRole(SelectedSortItem);
            for(int i = 0; i < EmployeeOb.Count; i++)
            {
                if (!EmployeeOb[i].Information.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    EmployeeOb.Remove(EmployeeOb[i]);
                }
            }
            timeDelay = 800;
        }

    }
}
