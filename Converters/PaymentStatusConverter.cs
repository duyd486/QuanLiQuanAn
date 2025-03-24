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
    public class PaymentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                string param = parameter as string;

                if (param == "Content")
                    return status == 2 ? "Đã thanh toán" : "Chưa thanh toán";

                if (param == "Enabled")
                    return status != 2; // 1: Nhấn được,  2: Không nhấn được
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
