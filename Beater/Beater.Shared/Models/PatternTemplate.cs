using Beater.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Beater.Models
{
    class PatternTemplate : INotifyPropertyChanged
    {
        public PatternTemplate(int BPM)
        {
            _timingDenominator = 4;
            _timingNumerator = 4;
            _beats = new bool[4];
            _bpm = BPM;
            Id = "#FFFFFF";
        }

        private readonly string[] timing_properties = new string[] { "BPM", "TimingDenominator", "BeatLength", "Beats", "TimingNumerator", "Measure" };

        private double _timingNumerator;
        public double TimingNumerator
        {
            get { return TimingNumerator; }
            set
            {
                _timingNumerator = value;
                var beatCount = (int)Math.Floor(_timingNumerator);
                Array.Resize(ref _beats, beatCount);
                RaisePropertyChanged(timing_properties);
            }
        }
        private double _timingDenominator;
        public double TimingDenominator
        {
            get { return _timingDenominator; }
            set
            {
                _timingDenominator = value;
                RaisePropertyChanged(timing_properties);
            }
        }
        public Sample.Count BeatLength { get { return (Sample.Count)((240.0 / _bpm) / _timingDenominator) * Sample.SamplesPerSecond; } }
        public Sample.Count Measure { get { return (Sample.Count)(((240.0 / _bpm) / _timingDenominator) * _timingNumerator) * Sample.SamplesPerSecond; } }
        private bool[] _beats;
        public bool[] Beats { get { return _beats; } set { _beats = value; } }
        public string Id { get; set; }

        //special case: this is set globally rather than per track, so it gets updated differently.
        private int _bpm;
        public int BPM
        {
            get { return _bpm; }
            set
            {
                _bpm = value;
                RaisePropertyChanged(timing_properties);
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
