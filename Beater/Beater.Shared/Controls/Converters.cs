using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Linq;

namespace Beater.Controls
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return NotFalseOrNull(value) ^ Invert ? Visibility.Visible : Visibility.Collapsed;
        }

        protected static bool NotFalseOrNull(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            else
            {
                return value != null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.Equals(Visibility.Visible) ^ Invert;
        }
    }

    public class BooleanInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }
    }

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
            return value;
        }
    }
}
