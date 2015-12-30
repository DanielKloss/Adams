using System;
using Windows.UI.Xaml.Data;

namespace TheClockEnd.Converters
{
    public class BigHandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime time = (DateTime)value;

            return time.Minute * 6;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
