using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rld.Acs.WpfApplication.View.Converter
{
    public class ToggleExpandToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool expand = (bool)value;
            switch (expand)
            {
                case true:
                    return Visibility.Visible;
                case false:
                    return Visibility.Hidden;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
