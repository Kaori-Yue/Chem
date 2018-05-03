using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Chem.Views.Converters
{
    class RadioBoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            if (String.IsNullOrEmpty(str))
                return Binding.DoNothing;
            if (str.ToString().Equals(parameter.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
            //if (parameter.ToString().Equals("L")))
            //    return "L";
            //return "R";
        }
    }
}
