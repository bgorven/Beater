using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Beater.Controls
{
    class CanvasItemsControl : ItemsControl
    {
        public string BindingPathLeft { get; set; }
        public string BindingPathTop { get; set; }
        public string BindingPathZ { get; set; }
        public IValueConverter BindingConverterPosition { get; set; }
        public IValueConverter BindingConverterZIndex { get; set; }
        public static readonly DependencyProperty BindingPathLeftProperty = DependencyProperty.Register("BindingPathLeft", typeof(string), typeof(CanvasItemsControl), null);
        public static readonly DependencyProperty BindingPathTopProperty = DependencyProperty.Register("BindingPathTop", typeof(string), typeof(CanvasItemsControl), null);
        public static readonly DependencyProperty BindingPathZProperty = DependencyProperty.Register("BindingPathZ", typeof(string), typeof(CanvasItemsControl), null);
        public static readonly DependencyProperty BindingConverterPositionProperty = DependencyProperty.Register("BindingConverterPosition", typeof(IValueConverter), typeof(CanvasItemsControl), null);
        public static readonly DependencyProperty BindingConverterZIndexProperty = DependencyProperty.Register("BindingConverterZIndex", typeof(IValueConverter), typeof(CanvasItemsControl), null);

        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            FrameworkElement contentControl = element as FrameworkElement;
            if (contentControl != null)
            {
                TryBind(contentControl, Canvas.LeftProperty, BindingPathLeft, BindingConverterPosition);
                TryBind(contentControl, Canvas.TopProperty, BindingPathTop, BindingConverterPosition);
                TryBind(contentControl, Canvas.ZIndexProperty, BindingPathZ, BindingConverterZIndex);
            }

            base.PrepareContainerForItemOverride(element, item);
        }
        

        private static void TryBind(FrameworkElement contentControl, DependencyProperty property, string BindingPath, IValueConverter Converter)
        {
            if (BindingPath != null)
            {
                Binding binding = new Binding() { Path = new PropertyPath(BindingPath) };
                if (Converter != null)
                {
                    binding.Converter = Converter;
                }
                contentControl.SetBinding(property, binding);
            }
        }
    }
}
