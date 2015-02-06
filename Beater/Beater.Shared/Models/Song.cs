using System;
using System.Collections.Generic;
using System.Text;
using Beater;

namespace Beater.Models
{
    class Song
    {
        private Sample.Count _length;

        public TimeSpan Length
        {
            get
            {
                return _length.Time();
            }
            set
            {
                _length = value.Samples();
            }
        }

        public int BPM { get; set; }

        public string Name { get; set; }

        public List<Track> Tracks = new List<Track>();
    }
}
