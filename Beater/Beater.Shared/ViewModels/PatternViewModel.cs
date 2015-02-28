using Beater.Audio;
using Beater.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Beater.ViewModels
{
    class PatternViewModel : ViewModelBase
    {
        private Pattern pattern;

        public PatternViewModel() : this(new Pattern(new PatternTemplate(60))) { }
        public PatternViewModel(Pattern model)
        {
            pattern = model;
            pattern.PropertyChanged += PropagatePropertyChanged;
            Beats = new ObservableCollection<BeatViewModel>(BeatViewModel.BeatPattern(pattern));
        }

        new void PropagatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Beats.Count != pattern.Beats.Length)
            {
                Beats.Clear();
                foreach (var beat in BeatViewModel.BeatPattern(pattern))
                {
                    Beats.Add(beat);
                }
            }
            base.PropagatePropertyChanged(sender, e);
        }

        

        public Sample.Count BeatLength { get { return pattern.BeatLength; } }
        public Sample.Count Measure { get { return pattern.Measure; } }
        public ObservableCollection<BeatViewModel> Beats { get; private set; }
        public Brush Id { get { return pattern.Color; } set { pattern.Color = value; } }
        public Sample.Count Location { get { return pattern.Location; } set { pattern.Location = value; } }
        public PatternTemplate Template { get { return pattern.Template; } set { pattern.Template = value; } }
    }
}
