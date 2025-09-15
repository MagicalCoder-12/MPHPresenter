using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MPHPresenter.Converters
{
    public class ContentTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string contentType && parameter is string targetTypes)
            {
                var types = targetTypes.Split(',');
                return types.Contains(contentType) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}