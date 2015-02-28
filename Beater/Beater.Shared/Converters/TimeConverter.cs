using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Beater.Converters
{
    [ContentProperty(Name="Formats")]
    class TimeConverter : IValueConverter
    {
        public List<string> Formats { get; set; }
        public string Type { get; set; }

        public TimeConverter()
        {
            Formats = new List<string>();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime) return ((DateTime)value).ToString(Formats[0]);
            if (value is TimeSpan) return ((TimeSpan)value).ToString(Formats[0]);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (Type == "DateTime")
            {
                DateTime result;
                if (DateTime.TryParse((string)value, out result))
                {
                    return result;
                }
                return value;
            }
            if (Type == "TimeSpan")
            {
                TimeSpan result;
                var invert = false;
                var val = (string)value;
                if (val.StartsWith("-"))
                {
                    invert = true;
                    val = val.Substring(1);
                }
                if (TimeSpan.TryParseExact(val, Formats.ToArray(), CultureInfo.CurrentCulture, out result))
                {
                    return invert ? -result : result;
                }
                if (TimeSpan.TryParse(val, out result))
                {
                    return invert ? -result : result;
                }
                return value;
            }
            return value;
        }
    }
}
