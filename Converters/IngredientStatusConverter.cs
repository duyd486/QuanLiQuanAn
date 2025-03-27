using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Data;

namespace QuanLiQuanAn.Converters
{
    public class IngredientStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                string param = parameter as string;

                if (param == "Content")
                    switch (status)
                    {
                        case 1:
                            return "⚠️";
                        case 2:
                            return "📦";
                        case 3:
                            return "✅";
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