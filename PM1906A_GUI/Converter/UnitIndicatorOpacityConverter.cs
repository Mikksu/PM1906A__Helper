using PM1906AHelper.Core;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PM1906A_GUI.Converter
{
    public class UnitIndicatorOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var unit = (UnitEnum)value;
            var unit_tag = parameter.ToString();
            if (unit.ToString() == unit_tag)
                return 0.3;
            else
                return 0.03;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
