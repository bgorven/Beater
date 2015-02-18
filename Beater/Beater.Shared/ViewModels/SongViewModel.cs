using System;
using System.Collections.Generic;
using System.Text;
using Beater.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.IO;
using System.Windows.Input;
using System.Linq;
using Windows.ApplicationModel;
using NAudio.Wave;
using NAudio.Win8.Wave.WaveOutputs;
using Beater.Audio;
using Windows.UI.Core;

namespace Beater.ViewModels
{
    class SongViewModel : ViewModelBase
    {
        private Song song = new Song { BPM = 60, Length = TimeSpan.FromSeconds(36).Samples(), Title = "Untitled", Tracks = new List<Track>() };

        #region Model Properties
        public int BPM
        {
            get { return song.BPM; }
            set
            {
                song.BPM = value;
                RaisePropertyChanged("BPM");
            }
        }

        public TimeSpan Length
        {
            get { return song.Length.Time(); }
            set
            {
                song.Length = value.Samples();
                RaisePropertyChanged("Length");
            }
        }

        public int SampleCount
        {
            get { return song.Length; }
        }

        public string Title
        {
            get { return song.Title; }
            set
            {
                song.Title = value;
                RaisePropertyChanged("Title");
            }
        }
        #endregion

        /// <summary>
        /// Creates sample data for testing/editor
        /// </summary>
        public SongViewModel() : this(null) { }

        public SongViewModel(string defaultWave)
        {
            if (defaultWave != null && !defaultWave.Contains("\\"))
            {
                defaultWave = Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\" + defaultWave + ".wav");
            }

            PlayCommand = new Command(CanPlay, Play);
            PauseCommand = new Command(CanPause, Pause);
            StopCommand = new Command(CanStop, Stop);
            SaveCommand = new Command(Save);
            TrashCommand = new Command(Trash);
            AddTrackCommand = new Command(CanAddTrack, AddTrack);

            Tracks = new ObservableCollection<TrackViewModel>(song.Tracks.Select(model => new TrackViewModel(model)));
            provider = new SongSampleProvider(this);

            if (song.Tracks.Count == 0)
            {
                song.Tracks.Add(new Track(defaultWave, Length, TimeSpan.Zero, BPM));
                Tracks.Add(new TrackViewModel(song.Tracks[0]));
                Tracks[0].Wave = Tracks[0].OriginalWave;
                Tracks[0].Pattern[0].Beats[0].Beats = true;
                Tracks[0].Pattern[0].Beats[2].Beats = true;
            }

            try
            {
                var window = CoreWindow.GetForCurrentThread();
                if (window != null) UIThread = window.Dispatcher;
            }
            catch (Exception) { }
        }

        private CoreDispatcher UIThread;

        public ObservableCollection<TrackViewModel> Tracks { get; private set; }

        #region Audio
        public Sample.Count Progress { get; private set; }
        public TimeSpan PlayProgress
        {
            get { return Progress.Time(); }
            set
            {
                Progress = value.Samples();
                RaisePropertyChanged("PlayProgress");
            }
        }

        private IWavePlayer player;
        private SongSampleProvider provider;

        internal void SetProgress(int p)
        {
            Progress = p % SampleCount;
            if (UIThread != null)
            {
                var ignore = UIThread.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    RaisePropertyChanged("PlayProgress");
                    RaisePropertyChanged("Progress");
                });
            }
        }

        #endregion

        #region Commands
        public Command PlayCommand { get; private set; }

        private bool CanPlay()
        {
            return player == null || player.PlaybackState != PlaybackState.Playing;
        }

        private async Task Play()
        {
            if (player == null)
            {
                player = await Task.Run(() =>
                {
                    var p = new WasapiOutRT(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 500);
                    p.Init(provider);
                    return p;
                });
            }

            player.Play();

            PlayCommand.Notify();
            PauseCommand.Notify();
            StopCommand.Notify();
        }

        public Command PauseCommand { get; private set; }

        private bool CanPause()
        {
            return player != null && player.PlaybackState == PlaybackState.Playing;
        }

        private Task Pause()
        {
            if (player != null)
            {
                player.Pause();
            }

            PlayCommand.Notify();
            PauseCommand.Notify();
            StopCommand.Notify();
            return nullTask;
        }

        public Command StopCommand { get; private set; }

        private bool CanStop()
        {
            return player != null && player.PlaybackState != PlaybackState.Stopped;
        }

        private Task Stop()
        {
            if (player != null)
            {
                player.Stop();
            }
            PlayProgress = TimeSpan.Zero;

            PlayCommand.Notify();
            PauseCommand.Notify();
            StopCommand.Notify();
            return nullTask;
        }

        public Command SaveCommand { get; private set; }

        private Task Save()
        {
            return nullTask;
        }

        public Command TrashCommand { get; private set; }

        private Task Trash()
        {
            return nullTask;
        }

        private string assetFile;
        public string AssetFile
        {
            get { return assetFile; }
            set
            {
                assetFile = value;
                AddTrackCommand.Notify();
            }
        }
        public Command AddTrackCommand { get; private set; }
        private bool CanAddTrack()
        {
            return AssetFile != null;
        }
        private async Task AddTrack()
        {
            var path = Path.Combine(Package.Current.InstalledLocation.Path, "Assets");
            path = Path.Combine(path, AssetFile + ".wav");
            var model = await Task.Run(() => new Track(path, Length, TimeSpan.Zero, BPM));
            var viewmodel = new TrackViewModel(model);
            await viewmodel.UpdateWave();
            Tracks.Add(viewmodel);
        }
        #endregion
    }
}
