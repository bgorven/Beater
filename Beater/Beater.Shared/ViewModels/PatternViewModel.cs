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
        private Pattern pattern;

        public override event PropertyChangedEventHandler PropertyChanged;

        public PatternViewModel() : this(new Pattern(new PatternTemplate(60))) { }
        public PatternViewModel(Pattern model)
        {
            pattern = model;
            pattern.PropertyChanged += this.PropertyChanged;
            Beats = new ObservableCollection<BeatViewModel>(BeatViewModel.BeatPattern(pattern));
        }

        public int BeatLength { get { return pattern.BeatLength; } }
        public int Measure { get { return pattern.Measure; } }
        public ObservableCollection<BeatViewModel> Beats { get; private set; }
        public string Id { get { return pattern.Id; } set { pattern.Id = value; } }
        public int Location
        {
            get { return pattern.Location; }
            set
            {
                pattern.Location = value;
                RaisePropertyChanged("Location");
            }
        }
    }
}
