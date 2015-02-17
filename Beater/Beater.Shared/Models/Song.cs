using System;
using System.Collections.Generic;
using System.Text;
using Beater;
using Beater.Audio;

namespace Beater.Models
{
    class Song
    {
        public Sample.Count Length { get; set; }

        public int BPM { get; set; }

        public string Title { get; set; }

        public List<Track> Tracks = new List<Track>();
    }
}
