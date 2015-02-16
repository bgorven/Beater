using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Beater.Models
{
    class Pattern : INotifyPropertyChanged, IDisposable
    {
        public PatternTemplate Template;

        public Pattern(PatternTemplate template)
        {
            Template = template;
            Template.PropertyChanged += PropertyChanged;
        }

        public void Dispose()
        {
            Template.PropertyChanged -= PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Sample.Count BeatLength
        {
            get { return Template.BeatLength; }
            set
            {
                Template.BeatLength = value;
                Template.RaisePropertyChanged("BeatLength");
            }
        }

        public Sample.Count Measure
        {
            get { return Template.Measure; }
            set
            {
                Template.Measure = value;
                Template.RaisePropertyChanged("Measure");
            }
        }

        /// <summary>
        /// Note: you need to call <code>RaiseBeatChanged()</code> if you set an array member; we've got some refactoring to do here.
        /// </summary>
        public bool[] Beats
        {
            get { return Template.Beats; }
            set
            {
                Template.Beats = value;
                Template.RaisePropertyChanged("Beats");
            }
        }

        public string Id
        {
            get { return Template.Id; }
            set
            {
                Template.Id = value;
                Template.RaisePropertyChanged("Id");
            }
        }

        public int BPM
        {
            get { return Template.BPM; }
            set
            {
                Template.BPM = value;
                Template.RaisePropertyChanged("BPM");
            }
        }

        internal void RaiseBeatsChanged()
        {
            Template.RaisePropertyChanged("Beats");
        }
    }
}
