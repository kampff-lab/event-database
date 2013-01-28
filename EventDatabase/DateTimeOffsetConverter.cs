using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace EventDatabase
{
    public class DateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTimeOffset = (DateTimeOffset)value;
            if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
            {
                return dateTimeOffset.LocalDateTime;
            }

            if (targetType == typeof(string))
            {
                return dateTimeOffset.ToString(culture);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueType = value == null ? typeof(object) : value.GetType();
            if (valueType == typeof(DateTime?))
            {
                value = ((DateTime?)value).GetValueOrDefault();
                valueType = typeof(DateTime);
            }

            if (valueType == typeof(DateTime))
            {
                var dateTime = (DateTime)value;
                DateTimeOffset utcDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                return utcDateTime;
            }

            if (valueType == typeof(string))
            {
                var dateTimeString = (string)value;
                return DateTimeOffset.Parse(dateTimeString, culture);
            }

            return Binding.DoNothing;
        }
    }
}
