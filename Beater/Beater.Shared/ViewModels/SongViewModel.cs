using System;
using System.Collections.Generic;
using System.Text;
using Beater.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.IO;

namespace Beater.ViewModels
{
    class SongViewModel : ViewModelBase
    {
        private Song song = new Song { BPM = 60, Length = TimeSpan.FromMinutes(3), Name = "Untitled" };

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

        public string Name
        {
            get { return song.Name; }
            set
            {
                song.Name = value;
                RaisePropertyChanged("Name");
            }
        }
        #endregion

        public SongViewModel()
        {
            PlayCommand = new Command(CanPlay, Play);
            PauseCommand = new Command(CanPause, Pause);
            StopCommand = new Command(CanStop, Stop);
            SaveCommand = new Command(Save);
            TrashCommand = new Command(Trash);
        }

        private bool playing;

        public ObservableCollection<TrackViewModel> tracks = new ObservableCollection<TrackViewModel>(); 

        #region Commands
        public Command PlayCommand;

        private bool CanPlay()
        {
            return !playing;
        }

        private Task Play()
        {
            playing = true;
            return nullTask;
        }

        public Command PauseCommand;

        private bool CanPause()
        {
            return playing;
        }

        private Task Pause()
        {
            playing = false;
            return nullTask;
        }

        public Command StopCommand;

        private bool CanStop()
        {
            return !playing;
        }

        private Task Stop()
        {
            playing = true;
            return nullTask;
        }

        public Command SaveCommand;

        private Task Save()
        {
            return nullTask;
        }

        public Command TrashCommand;

        private Task Trash()
        {
            return nullTask;
        }

        private async Task AddTrack(StorageFile file)
        {
            var provider = await Task.Run(() => new Sample.Provider(file.Path));
            var model = new Track
            {
                Wave = provider,
                BPM = BPM,
                Filename = file.Path,
                Name = Path.GetFileNameWithoutExtension(file.Name),
                Length = Length.Samples(),
                Offset = 0,
            };
            tracks.Add(new TrackViewModel { track = model });
        }
        #endregion
    }
}
