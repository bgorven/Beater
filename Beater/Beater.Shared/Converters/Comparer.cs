using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace Beater.Converters
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name = "Comparisons")]
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
}
