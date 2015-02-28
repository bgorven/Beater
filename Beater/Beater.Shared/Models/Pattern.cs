using Beater.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Beater.Models
{
    class Pattern : INotifyPropertyChanged, IDisposable
    {
        public PatternTemplate Template { get; set; }

        public Pattern(PatternTemplate template)
        {
            Template = template;
            Template.PropertyChanged += PropagatePropertyChanged;
        }

        public void Dispose()
        {
            Template.PropertyChanged -= PropagatePropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Sample.Count BeatLength
        {
            get { return Template.BeatLength; }
        }

        public Sample.Count Measure
        {
            get { return Template.Measure; }
        }

        /// <summary>
        /// Note: you need to call <code>RaiseBeatChanged()</code> if you set an array member; we've got some refactoring to do here.
        /// </summary>
        public bool[] Beats
        {
            get { return Template.Beats; }
            set { Template.Beats = value; }
        }

        public Sample.Provider Wave
        {
            get { return Template.Wave; }
            set { Template.Wave = value; }
        }

        public Brush Color
        {
            get { return Template.Color; }
            set { Template.Color = value; }
        }

        public double BPM
        {
            get { return Template.BPM; }
            set { Template.BPM = value; }
        }

        internal void RaiseBeatsChanged()
        {
            Template.RaisePropertyChanged("Beats");
        }

        internal void PropagatePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null && args != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(args.PropertyName));
            }
        }

        private Sample.Count _location;
        public Sample.Count Location
        {
            get { return _location; }
            set
            {
                _location = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Location"));
                }
            }
        }
    }
}
