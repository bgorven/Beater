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

namespace Beater.ViewModels
{
    class SongViewModel : ViewModelBase
    {
        private Song song = new Song { BPM = 60, Length = TimeSpan.FromMinutes(3), Title = "Untitled", Tracks = new List<Track>() };

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
            get { return song.Length; }
            set
            {
                song.Length = value;
                RaisePropertyChanged("Length");
            }
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
        public SongViewModel()
        {
            PlayCommand = new Command(CanPlay, Play);
            PauseCommand = new Command(CanPause, Pause);
            StopCommand = new Command(CanStop, Stop);
            SaveCommand = new Command(Save);
            TrashCommand = new Command(Trash);
            AddTrackCommand = new Command(CanAddTrack, AddTrack);

            Tracks = new ObservableCollection<TrackViewModel>(song.Tracks.Select(model => new TrackViewModel(model)));

            string kick = null; //Path.Combine(Package.Current.InstalledLocation.Path, "Assets\\Kick.wav")

            if (song.Tracks.Count == 0)
            {
                song.Tracks.Add(new Track(kick, Length, TimeSpan.Zero, BPM));
                Tracks.Add(new TrackViewModel(song.Tracks[0]));
            }
        }

        private bool playing;

        public ObservableCollection<TrackViewModel> Tracks { get; private set; } 

        #region Commands
        public Command PlayCommand { get; private set; }

        private bool CanPlay()
        {
            return !playing;
        }

        private async Task Play()
        {
            playing = true;

            await nullTask;
            PauseCommand.Notify();
            StopCommand.Notify();
        }

        public Command PauseCommand { get; private set; }

        private bool CanPause()
        {
            return playing;
        }

        private Task Pause()
        {
            playing = false;
            PlayCommand.Notify();
            StopCommand.Notify();
            return nullTask;
        }

        public Command StopCommand { get; private set; }

        private bool CanStop()
        {
            return !playing;
        }

        private Task Stop()
        {
            playing = false;
            PlayCommand.Notify();
            PauseCommand.Notify();
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
            Tracks.Add(new TrackViewModel(model));
        }
        #endregion
    }
}
