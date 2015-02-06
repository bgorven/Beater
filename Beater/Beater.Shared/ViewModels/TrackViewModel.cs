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
        internal Track track = new Track() { Name = "", Filename = "", Offset = 0, BPM = 60 };

        #region Model Properties
        string Name
        {
            get { return track.Name; }
            set
            {
                track.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        string Filename
        {
            get { return track.Filename; }
            set
            {
                track.Filename = value;
                RaisePropertyChanged("Filename");
            }
        }

        long Offset
        {
            get { return track.Offset; }
            set
            {
                track.Offset = value;
                RaisePropertyChanged("Offset");
            }
        }

        int BPM
        {
            get { return track.BPM; }
            set
            {
                track.BPM = value;
                RaisePropertyChanged("BPM");
            }
        }

        long Length
        {
            get { return track.Length; }
            set
            {
                track.Length = value;
                RaisePropertyChanged("Length");
                for (long t = Offset, i = 0; t < Length ; t += pattern[(int)i].Measure, i++ )
                {
                    if (pattern.Count <= i)
                    {
                        if (track.Templates.Count == 0) track.Templates.Add(new BeatTemplate(BPM));
                    }
                }
            }
        }

        Sample.Provider Wave
        {
            get { return track.Wave; }
            set
            {
                track.Wave = value;
                RaisePropertyChanged("Wave");
            }
        }
        #endregion

        ObservableCollection<BeatViewModel> pattern = new ObservableCollection<BeatViewModel>();
    }
}
