using Beater.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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

        internal void PropagatePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null && args != null)
            {
                PropertyChanged(this, args);
            }
        }
    }
}
