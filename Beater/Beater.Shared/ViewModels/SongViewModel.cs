﻿using System;
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

        public SongViewModel()
        {
            PlayCommand = new Command(CanPlay, Play);
            PauseCommand = new Command(CanPause, Pause);
            StopCommand = new Command(CanStop, Stop);
            SaveCommand = new Command(Save);
            TrashCommand = new Command(Trash);

            Tracks = new ObservableCollection<TrackViewModel>(song.Tracks.Select(model => new TrackViewModel(model)));

            if (song.Tracks.Count == 0)
            {
                song.Tracks.Add(new Track(null, Length, TimeSpan.Zero, BPM));
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

        private async Task AddTrack(StorageFile file)
        {
            var model = await Task.Run(() => new Track(file.Path, Length, TimeSpan.Zero, BPM));
            Tracks.Add(new TrackViewModel(model));
        }
        #endregion
    }
}
