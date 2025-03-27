using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace QuanLiQuanAn.Converters
{
    public class RoleStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.ToString(), out int role))
            {
                switch (role)
                {
                    case 1:
                        return "Phục vụ";
                    case 2:
                        return "Nhân viên";
                    case 3:
                        return "Đầu bếp";
                    default:
                        return "Không xác định";
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}