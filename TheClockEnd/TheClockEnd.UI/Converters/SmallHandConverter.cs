using System;
using Windows.UI.Xaml.Data;

namespace TheClockEnd.UI.Converters
{
    public class SmallHandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime time = (DateTime)value;

            int hour = int.Parse(time.ToString("%h")) * 30;
            int minute;

            if (time.Minute >= 45)
            {
                minute = 18;
            }
            else if (time.Minute >= 30)
            {
                minute = 12;
            }
            else if (time.Minute >= 15)
            {
                minute = 6;
            }
            else
            {
                minute = 0;
            }

            return hour + minute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
