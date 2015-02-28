using Beater.Audio;
using Beater.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Beater.Views
{
    public sealed partial class WaveLine : Path
    {
        public WaveLine()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty WaveProperty = DependencyProperty.Register("Wave", typeof(float[]), typeof(WaveLine), new PropertyMetadata(null, UpdatePoints));

        public float[] Wave {
            get { return (float[])GetValue(WaveProperty); }
            set { SetValue(WaveProperty, value); }
        }

        public static readonly DependencyProperty SamplesToPixelsConverterProperty = DependencyProperty.Register("SamplesToPixelsConverter", typeof(NumericScaleConverter), typeof(WaveLine), new PropertyMetadata(null, UpdatePoints));

        public NumericScaleConverter SamplesToPixelsConverter
        {
            get { return (NumericScaleConverter)GetValue(SamplesToPixelsConverterProperty); }
            set { SetValue(SamplesToPixelsConverterProperty, value); }
        }

        public static readonly DependencyProperty YScaleProperty = DependencyProperty.Register("YScale", typeof(double), typeof(WaveLine), new PropertyMetadata(1.0, UpdatePoints));

        public double YScale
        {
            get { return (double)GetValue(YScaleProperty); }
            set { SetValue(YScaleProperty, value); }
        }

        private static async void UpdatePoints(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            var w = d as WaveLine;
            
            if (w != null && w.SamplesToPixelsConverter != null)
            {
                var wave = w.Wave ?? new Beater.SampleData.WaveSampleData().Points;
                var xScale = (float)w.SamplesToPixelsConverter.Scale;
                var yScale = (float)w.YScale;

                var points = new PointCollection();
                foreach (var point in await Task.Run(() => Update(wave, xScale, yScale)))
                {
                    points.Add(point);
                }

                w.PolyBezier.Clear();
                w.PolyBezier.Add(new PolyBezierSegment() { Points = points });
            }
        }

        private static List<Point> Update(float[] wave, float xScale, float yScale)
        {
            var count = (int)(wave.Length * xScale);
            var pixels = new float[count];
            var list = new List<Point>();

            float max = 0;
            for (int i = 0, j = 0; i < count; i++){
                float blockMax = 0;
                for (  ; j * xScale < i+1 && j < wave.Length; j++){
                    blockMax = Math.Max(wave[j], blockMax);
                }
                max = Math.Max(max, blockMax);
                pixels[i] = blockMax;
            }

            Point prev = new Point(0,0);
            foreach (var curr in FindInflectionPoints(pixels, yScale/max))
            {
                var centerX = (prev.X + curr.X)/2;
                var control0 = new Point(centerX, prev.Y);
                var control1 = new Point(centerX, curr.Y);

                //two bezier handles and an endpoint;
                list.Add(control0);
                list.Add(control1);
                list.Add(curr);

                prev = curr;
            }

            return list;
        }

        static IEnumerable<Point> FindInflectionPoints(float[] points, float scaleFactor = 1)
        {
            bool findMax = true;
            float prev = 0;

            for (var i = 0; i < points.Length; i++)
            {
                var curr = points[i];
                if (curr > prev ^ findMax)
                {
                    yield return new Point(i, prev * scaleFactor);
                    findMax = !findMax;
                }
                prev = curr;
            }
        }
    }
}
