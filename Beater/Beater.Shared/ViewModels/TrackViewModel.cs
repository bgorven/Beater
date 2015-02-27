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

        public TrackViewModel() : this(new Track())
        {
            UpdatePattern();
            Pattern[0].Beats[1].Beats = true;
        }

        public TrackViewModel(Track model)
        {
            track = model;
            Pattern = new ObservableCollection<PatternViewModel>(model.Pattern.Select(beat => new PatternViewModel(beat)));
            Provider = new TrackSampleProvider(this);
            PropertyChanged += TrackViewModel_PropertyChanged;
        }

        async void TrackViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await Update();
        }

        private PatternTemplate _currentTemplate;
        public PatternTemplate CurrentTemplate
        {
            get { return _currentTemplate; }
            set
            {
                _currentTemplate = value;
                RaisePropertyChanged("CurrentTemplate");
            }
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

        public Sample.Count Offset
        {
            get { return track.Offset; }
            set
            {
                track.Offset = value;
                SetChanges();
                RaisePropertyChanged("Offset");
            }
        }

        public TimeSpan OffsetTime
        {
            get { return Offset.Time(); }
            set
            {
                Offset = value.Samples();
                RaisePropertyChanged("OffsetTime");
            }
        }

        public double BPM
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

        private bool _pendingChanges;
        public bool PendingChanges
        {
            get { return _pendingChanges || track.Templates.Any(t => t.PendingChanges); }
        }
        public void SetChanges() { _pendingChanges = true; }
        public void ClearChanges()
        {
            _pendingChanges = false;
            foreach (var t in track.Templates) t.PendingChanges = false;
        }

        private string _previousFileName = null;
        public async Task Update()
        {
            if (Filename != null && Filename != _previousFileName)
            {
                _previousFileName = Filename;
                try
                {
                    OriginalWave = await Task.Run(() => new Sample.Provider(Filename));
                    Wave = await Task.Run(() => UpdateWave(OriginalWave));
                }
                catch (Exception é)
                {
                    Logger.Log(é);
                }
            }

            UpdatePattern();
        }

        public void UpdatePattern()
        {
            foreach (var template in track.Templates)
            {
                if (template.BPM != BPM) template.BPM = BPM;
            }
            if (track.Templates.Count == 0)
            {
                AddTemplate();
                CurrentTemplate = track.Templates[0];
            }

            if (!PendingChanges) return;
            ClearChanges();

            int location = 0;
            foreach (var pattern in Pattern)
            {
                pattern.Location = location;
                location += pattern.Measure;
            }

            while (location < Length)
            {
                var pattern = new Pattern(track.Templates[0]);
                track.Pattern.Add(pattern);
                Pattern.Add(new PatternViewModel(pattern) { Location = location });
                location += pattern.Measure;
            }
        }

        private void AddTemplate()
        {
            var template = new PatternTemplate(BPM);
            template.PropertyChanged += TrackViewModel_PropertyChanged;
            track.Templates.Add(template);
        }

        private Sample.Provider UpdateWave(Sample.Provider original)
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
        }

        public ObservableCollection<PatternViewModel> Pattern { get; private set; }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
            }
        }
    }
}
