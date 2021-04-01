using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPFCovidItalyAnalizer.Model;

namespace WPFCovidItalyAnalizer.Converter
{
    public class IntegerToEnum : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(DateRange))
                return (int)Enum.Parse(typeof(DateRange), value.ToString());

            return (int)0;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(DateRange))
                return (DateRange)(int)value;

            return DateRange.LastTwoWeeks;
        }
    }
}
