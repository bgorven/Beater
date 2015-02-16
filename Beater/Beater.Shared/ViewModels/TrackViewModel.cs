using Beater.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Beater.ViewModels
{
    class TrackViewModel : ViewModelBase
    {
        Track track;

        public TrackViewModel(Track model)
        {
            track = model;
            Pattern = new ObservableCollection<PatternViewModel>(model.Pattern.Select(beat => new PatternViewModel(beat)));
        }

        #region Model Properties
        public string Name
        {
            get { return track.Name; }
            set
            {
                track.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Filename
        {
            get { return track.Filename; }
            set
            {
                track.Filename = value;
                RaisePropertyChanged("Filename");
            }
        }

        public long Offset
        {
            get { return track.Offset; }
            set
            {
                track.Offset = value;
                RaisePropertyChanged("Offset");
            }
        }

        public int BPM
        {
            get { return track.BPM; }
            set
            {
                track.BPM = value;
                RaisePropertyChanged("BPM");
            }
        }

        public long Length
        {
            get { return track.Length; }
            set
            {
                track.Length = value;
                RaisePropertyChanged("Length");
                for (long t = Offset, i = 0; t < Length ; t += Pattern[(int)i].Measure, i++ )
                {
                    if (Pattern.Count <= i)
                    {
                        if (track.Templates.Count == 0) track.Templates.Add(new PatternTemplate(BPM));
                    }
                }
            }
        }

        public Sample.Provider Wave
        {
            get { return track.Wave; }
            set
            {
                track.Wave = value;
                RaisePropertyChanged("Wave");
            }
        }
        #endregion

        public ObservableCollection<PatternViewModel> Pattern { get; private set; }
    }
}
