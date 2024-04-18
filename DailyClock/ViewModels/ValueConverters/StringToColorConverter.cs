using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DailyClock.ViewModels.ValueConverters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string str) throw new ArgumentException("类型错误");

            str = str.Replace("#", "");

            string sa = str.Substring(0, 2);
            string sr = str.Substring(2, 2);
            string sg = str.Substring(4, 2);
            string sb = str.Substring(6, 2);

            byte a = byte.Parse(sa, NumberStyles.HexNumber);
            byte r = byte.Parse(sr, NumberStyles.HexNumber);
            byte g = byte.Parse(sg, NumberStyles.HexNumber);
            byte b = byte.Parse(sb, NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Color clr) throw new ArgumentException("类型错误");

            string sa = clr.A.ToString("X2");
            string sr = clr.R.ToString("X2");
            string sg = clr.G.ToString("X2");
            string sb = clr.B.ToString("X2");

            return $"#{sa}{sr}{sg}{sb}";
        }
    }
}
