using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLiQuanAn.Converters
{
    public class TotalSalaryConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                double.TryParse(values[0]?.ToString(), out double num1) &&
                double.TryParse(values[1]?.ToString(), out double num2))
            {
                return (num1 * num2).ToString();
            }
            return "Lỗi";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
