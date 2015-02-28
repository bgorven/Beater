using Beater.Audio;
using System;
using Windows.UI.Xaml.Data;

namespace Beater.Converters
{
    public class NumericScaleConverter : IValueConverter
    {
        public double Scale { get; set; }

        public NumericScaleConverter()
        {
            Scale = 1;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var scale = GetScale(parameter);

            if (value is int) return (int)value * scale;
            if (value is long) return (long)value * scale;
            if (value is float) return (float)value * scale;
            if (value is double) return (double)value * scale;
            if (value is decimal) return (decimal)value * (decimal)scale;
            if (value is Sample.Count) return (Sample.Count)value * scale;
            return value;
        }

        private double GetScale(object parameter)
        {
            var p = parameter as string;
            double scale;

            if (!string.IsNullOrEmpty(p))
            {
                if (!double.TryParse(p, out scale))
                {
                    scale = Scale;
                }
            }
            else if (parameter is double)
            {
                scale = (double)parameter;
            }
            else
            {
                scale = Scale;
            }
            return scale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var scale = GetScale(parameter);

            if (value is int) return (int)value / scale;
            if (value is long) return (long)value / scale;
            if (value is float) return (float)value / scale;
            if (value is double) return (double)value / scale;
            if (value is decimal) return (decimal)value / (decimal)scale;
            if (value is Sample.Count) return (Sample.Count)value / scale;
            return value;
        }
    }
}