using Beater.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Beater.Audio;

namespace Beater.ViewModels
{
    class BeatViewModel : ViewModelBase
    {

        public static IEnumerable<BeatViewModel> BeatPattern(Pattern beat)
        {
            return beat.Beats.Select((b, i) => new BeatViewModel(beat, i));
        }

        public BeatViewModel() : this(new Pattern(new PatternTemplate()), 0) { }

        private BeatViewModel(Pattern pattern, int index)
        {
            _pattern = pattern;
            _beats = pattern.Beats;
            Index = index;
            pattern.PropertyChanged += pattern_PropertyChanged;
        }

        void pattern_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Beats") RaisePropertyChanged("Active");
            else if (e.PropertyName == "Wave") RaisePropertyChanged("Wave");
            else if (e.PropertyName == "Color") RaisePropertyChanged("Color");
            else if (e.PropertyName == "BeatLength") RaisePropertyChanged(new string[] { "BeatLength", "Location" });
        }

        private Pattern _pattern;
        private bool[] _beats;

        public int Index { get; private set; }

        public bool Active
        {
            get { return _beats[Index]; }
            set
            {
                _beats[Index] = value;
                _pattern.RaiseBeatsChanged();
            }
        }

        public float[] Wave { get { return _pattern.Wave == null ? new float[0] : _pattern.Wave.Samples; } }

        public Brush Color { get { return _pattern.Color; } }

        public Sample.Count BeatLength { get { return _pattern.BeatLength; } }

        public Sample.Count Location { get { return BeatLength * Index; } }
    }
}
