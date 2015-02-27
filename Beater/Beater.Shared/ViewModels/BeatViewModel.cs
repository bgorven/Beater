using Beater.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace Beater.ViewModels
{
    class BeatViewModel : ViewModelBase
    {
        public static IEnumerable<BeatViewModel> BeatPattern(Pattern beat)
        {
            return beat.Beats.Select((b, i) => new BeatViewModel(beat, i));
        }

        private BeatViewModel(Pattern pattern, int number)
        {
            _pattern = pattern;
            _beats = pattern.Beats;
            _index = number;
            pattern.PropertyChanged += PropagatePropertyChanged;
        }

        private Pattern _pattern;
        private bool[] _beats;
        private int _index;

        public bool Beats
        {
            get { return _beats[_index]; }
            set
            {
                _beats[_index] = value;
                _pattern.RaiseBeatsChanged();
            }
        }

        public int BeatLength { get { return _pattern.BeatLength; } }

        public int Location { get { return BeatLength * _index; } }
    }
}
