using Beater.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI;

namespace Beater.Models
{
    class PatternTemplate : INotifyPropertyChanged
    {
        public PatternTemplate() : this(60) { }

        public PatternTemplate(double BPM)
        {
            _timingDenominator = 4;
            _timingNumerator = 4;
            _beats = new bool[4];
            _bpm = BPM;
            var color = new byte[3];
            _rand.NextBytes(color);
            Id = Windows.UI.Color.FromArgb(0, color[0], color[1], color[2]);
            PendingChanges = true;
        }

        private Sample.Provider _wave;
        public Sample.Provider Wave { 
            get { return _wave; }
            set
            {
                _wave = value;
                RaisePropertyChanged("Wave");
            }
        }

        private static readonly Random _rand = new Random();

        private double _timingNumerator;
        public double TimingNumerator
        {
            get { return _timingNumerator; }
            set
            {
                _timingNumerator = value;
                var beatCount = (int)Math.Floor(_timingNumerator);
                Array.Resize(ref _beats, beatCount);
                TimingChanged();
            }
        }

        /// <summary>
        /// Set by the template when in is changed, cleared by the viewmodel when it updates.
        /// </summary>
        public bool PendingChanges { get; set; }
        private void TimingChanged()
        {
            PendingChanges = true;
            RaisePropertyChanged(timing_properties);
        }
        private static readonly string[] timing_properties = new string[] { "BPM", "TimingDenominator", "BeatLength", "Beats", "TimingNumerator", "Measure", "Location" };

        private double _timingDenominator;
        public double TimingDenominator
        {
            get { return _timingDenominator; }
            set
            {
                _timingDenominator = value;
                TimingChanged();
            }
        }
        public Sample.Count BeatLength { get { return (Sample.Count)(((240.0 / _bpm) / _timingDenominator) * Sample.SamplesPerSecond); } }
        public Sample.Count Measure { get { return (Sample.Count)((((240.0 / _bpm) / _timingDenominator) * _timingNumerator) * Sample.SamplesPerSecond); } }
        private bool[] _beats;
        public bool[] Beats { get { return _beats; } set { _beats = value; } }
        private Color _id;
        public Color Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        //special case: this is set globally rather than per track, so it gets updated differently.
        private double _bpm;
        public double BPM
        {
            get { return _bpm; }
            set
            {
                _bpm = value;
                TimingChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void RaisePropertyChanged(string[] changedProperties)
        {
            if (changedProperties != null)
            {
                foreach (var name in changedProperties)
                {
                    RaisePropertyChanged(name);
                }
            }
        }
    }
}
