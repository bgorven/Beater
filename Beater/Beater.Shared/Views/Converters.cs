using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Linq;
using Beater.Audio;

namespace Beater.Views
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name="Comparisons")]
    public class Comparer : IValueConverter
    {
        public object Default { get; set; }

        public List<ComparerItem> Comparisons { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            foreach (var c in Comparisons)
            {
                if (c.Compare(value, c.CompareTo)) return c.ResultIfEqual;
            }

            return Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            foreach (var c in Comparisons)
            {
                if (c.Compare(value, c.ResultIfEqual)) return c.ResultIfEqual;
            }

            return Default;
        }
    }

    public class ComparerItem
    {
        public object CompareTo { get; set; }

        public object ResultIfEqual { get; set; }

        public bool ReferenceEquality { get; set; }

        public bool Compare(object left, object right)
        {
            if (ReferenceEquality) return left == right;
            else if (left == null) return right == null;
            else return left.Equals(right);
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
