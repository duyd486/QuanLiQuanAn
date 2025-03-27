using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QuanLiQuanAn.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id && parameter is string type)
            {
                string imagePath = $"/Assets/Images/{type}/{id}.png";
                string defaulthPath = $"/Assets/Images/{type}/{type}Default.png";
                try
                {
                    Uri uri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                    App.GetResourceStream(uri);
                    return new BitmapImage(uri);
                }
                catch
                {
                    return new BitmapImage(new Uri(defaulthPath, UriKind.Relative));
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
