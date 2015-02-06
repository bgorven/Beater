using Beater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Beater.ViewModels
{
    class BeatViewModel : ViewModelBase
    {
        private Beat beat;
        private float lengthScale;

        public override event PropertyChangedEventHandler PropertyChanged;

        BeatViewModel(BeatTemplate template, float lengthScale)
        {
            beat = new Beat(template);
            beat.PropertyChanged += this.PropertyChanged;
            this.lengthScale = lengthScale;
        }

        public Sample.Count BeatLength { get { return beat.BeatLength; } set { beat.BeatLength = value; } }
        public Sample.Count Measure { get { return beat.Measure; } set { beat.Measure = value; } }
        public bool[] Beats { get { return beat.Beats; } set { beat.Beats = value; } }
        public string Id { get { return beat.Id; } set { beat.Id = value; } }
    }
}
