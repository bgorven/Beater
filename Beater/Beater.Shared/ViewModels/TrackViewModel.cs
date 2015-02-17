using Beater.Audio;
using Beater.Models;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beater.ViewModels
{
    class TrackViewModel : ViewModelBase
    {
        Track track;

        public TrackViewModel(Track model)
        {
            track = model;
            Pattern = new ObservableCollection<PatternViewModel>(model.Pattern.Select(beat => new PatternViewModel(beat)));
            Provider = new TrackSampleProvider(this);
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

        public int Offset
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

        public int Length
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

        public TrackSampleProvider Provider;

        public Sample.Provider Wave
        {
            get { return track.Wave; }
            set
            {
                track.Wave = value;
                RaisePropertyChanged("Wave");
            }
        }

        public Sample.Provider OriginalWave
        {
            get { return track.OriginalWave; }
            set
            {
                track.OriginalWave = value;
                RaisePropertyChanged("OriginalWave");
            }
        }
        #endregion

        public async Task UpdateWave()
        {
            var original = OriginalWave;

            Wave = await Task.Run(() =>
            {
                ISampleProvider processed = new Sample.Provider(original.Samples, original.WaveFormat);

                if (original.WaveFormat.SampleRate != Sample.SamplesPerSecond)
                {
                    processed = new WdlResamplingSampleProvider(processed, Sample.SamplesPerSecond);
                }

                if (processed.WaveFormat.Channels == 1)
                {
                    processed = new MonoToStereoSampleProvider(processed);
                }
                else if (processed.WaveFormat.Channels > 2)
                {
                    throw new FormatException(processed.WaveFormat.Channels + " channel audio? What am I supposed to do with that?");
                }

                var newBufSize = (int)(original.Samples.Length
                    * ((float)Sample.SamplesPerSecond / original.WaveFormat.SampleRate)
                    * (2 / original.WaveFormat.Channels)
                    * 2);

                var newBuf = new float[newBufSize];
                newBufSize = processed.Read(newBuf, 0, newBufSize);
                Array.Resize(ref newBuf, newBufSize);
                return new Sample.Provider(newBuf, Sample.WaveFormat);
            });
        }

        public ObservableCollection<PatternViewModel> Pattern { get; private set; }
    }
}
