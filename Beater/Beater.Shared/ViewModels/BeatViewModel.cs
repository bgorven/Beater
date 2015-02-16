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

        private BeatViewModel(Pattern beat, int number)
        {
            model = beat;
            index = number;
            beat.PropertyChanged += this.PropertyChanged;
        }

        private Pattern model;
        private int index;
        public long BeatLength { get { return model.BeatLength; } }
        public bool Active
        {
            get { return model.Beats[index]; }
            set
            {
                model.Beats[index] = value;
                model.RaiseBeatsChanged();
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
