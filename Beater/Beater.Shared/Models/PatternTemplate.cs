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
            BeatLength = TimeSpan.FromMinutes(1).Samples() / BPM;
            Measure = 4 * BeatLength;
            Beats = new bool[4];
            Id = "#FFFFFF";
            bpm = BPM;
        }

        public Sample.Count BeatLength { get; set; }
        public Sample.Count Measure { get; set; }
        public bool[] Beats { get; set; }
        public string Id { get; set; }

        //special case: this is set globally rather than per track, so it gets updated differently.
        private int bpm;
        private string[] props;
        public int BPM
        {
            get { return bpm; }
            set
            {
                var ratio = ((double)value) / bpm;
                bpm = value;
                BeatLength = (long)(BeatLength * ratio);
                Measure = (long)(Measure * ratio);
                props = props ?? new string[] { "BPM", "BeatLength", "Measure" };
                RaisePropertyChanged(props);
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
