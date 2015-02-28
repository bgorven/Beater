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
using Beater.Views;
using System.Threading;

namespace Beater.ViewModels
{
    class SongViewModel : ViewModelBase
    {
        private Song song = new Song { BPM = 60, Length = TimeSpan.FromSeconds(36).Samples(), Title = "Untitled", Tracks = new List<Track>() };

        public SongViewModel()
        {
            string defaultWave = "ms-appx:///Assets/Kick.wav";

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
                song.Tracks.Add(new Track(defaultWave, Length, 0, BPM));
                Tracks.Add(new TrackViewModel(song.Tracks[0]));
                Tracks[0].UpdatePattern();
                Tracks[0].Pattern[0].Beats[0].Active = true;
            }

            try
            {
                var window = CoreWindow.GetForCurrentThread();
                if (window != null) UIThread = window.Dispatcher;
                var ignoreResult = UIThread.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    foreach (var track in Tracks)
                    {
                        await track.Update();
                    }
                });
            }
            catch (Exception é)
            {
                Logger.Log(é);
            }
        }


        #region Model Properties
        public double BPM
        {
            get { return song.BPM; }
            set
            {
                song.BPM = value;
                foreach (var track in Tracks) track.BPM = value;
                RaisePropertyChanged("BPM");
            }
        }

        public TimeSpan Time
        {
            get { return song.Length.Time(); }
            set
            {
                var length = value.Samples();
                song.Length = length;
                foreach (var track in Tracks) track.Length = length;
                RaisePropertyChanged(new string[] { "Time", "Length" });
            }
        }

        public int Length
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

        private CoreDispatcher UIThread;

        public ObservableCollection<TrackViewModel> Tracks { get; private set; }

        #region Audio
        public int Progress { get; private set; }
        public TimeSpan PlayProgress
        {
            get { return ((Sample.Count)Progress).Time(); }
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
            Progress = p % Length;
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

            player.PlaybackStopped += player_PlaybackStopped;

            player.Play();

            PlayCommand.Notify();
            PauseCommand.Notify();
            StopCommand.Notify();
        }

        void player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            var ignore = UIThread.RunAsync(CoreDispatcherPriority.High, async () => await Stop());
            if (e.Exception != null)
            {
                var temp = Interlocked.Exchange(ref player, null);
                temp.Dispose();
            }
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
            var filename = Path.Combine(Package.Current.InstalledLocation.Path, "Assets");
            filename = Path.Combine(filename, AssetFile + ".wav");

            var viewmodel = new TrackViewModel(new Track(filename, Length, 0, BPM));
            await viewmodel.Update();
            Tracks.Add(viewmodel);
        }
        #endregion
    }
}
