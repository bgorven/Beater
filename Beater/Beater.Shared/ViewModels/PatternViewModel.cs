using Beater.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Beater.ViewModels
{
    class PatternViewModel : ViewModelBase
    {
        private Pattern beat;

        public override event PropertyChangedEventHandler PropertyChanged;

        public PatternViewModel(Pattern model)
        {
            beat = model;
            beat.PropertyChanged += this.PropertyChanged;
            Beats = new ObservableCollection<BeatViewModel>(BeatViewModel.BeatPattern(beat));
        }

        public int BeatLength { get { return beat.BeatLength; } set { beat.BeatLength = value; } }
        public int Measure { get { return beat.Measure; } set { beat.Measure = value; } }
        public ObservableCollection<BeatViewModel> Beats { get; private set; }
        public string Id { get { return beat.Id; } set { beat.Id = value; } }
    }
}
