using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TheClockEnd.UI.Converters
{
    class InverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool result = (bool)value;

            if (result)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
