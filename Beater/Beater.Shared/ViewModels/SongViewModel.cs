using System;
using System.Collections.Generic;
using System.Text;
using Beater.Models;

namespace Beater.ViewModels
{
    class SongViewModel : ViewModelBase
    {
        private Song song = new Song { BPM = 60, Length = TimeSpan.FromMinutes(3), Name = "Untitled" };

        public int BPM
    }
}
