using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Beater.Converters
{

    public class ColorConverter : IValueConverter
    {
        public static readonly Dictionary<string, Color> ColorNames = new Dictionary<string, Color>();

        static ColorConverter()
        {
            foreach (var color in typeof(Colors).GetRuntimeProperties())
            {
                ColorNames[color.Name] = (Color)color.GetValue(null);
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var asString = value as string;
            if (asString == null) return value;
            if (ColorNames.ContainsKey(asString)) return ColorNames[asString];
            if (!asString.StartsWith("#")) return asString;

            var hexString = asString.Trim().Trim(new char[] { '#' });

            byte[] bytes = new byte[8];

            if (hexString.Length == 3)
            {
                bytes[0] = 255;
                for (int i = 0; i < 3; i++)
                {
                    var digit = hexString.Substring(i, 1);
                    bytes[i + 1] = System.Convert.ToByte(digit + digit, 16);
                }
            }
            if (hexString.Length == 6)
            {
                bytes[0] = 255;
                for (int i = 0; i < 3; i++)
                {
                    bytes[i + 1] = System.Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
            }
            else if (hexString.Length == 8)
            {
                for (int i = 0; i < 4; i++)
                {
                    bytes[i] = System.Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
            }
            else return value;

            return Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }
    }
}
