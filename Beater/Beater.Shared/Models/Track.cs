using Beater.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Beater.Models
{
    class Track
    {
        public Track() : this("Default", TimeSpan.FromSeconds(36), TimeSpan.Zero, 60) { }

        public Track(string filename, TimeSpan length, TimeSpan offset, int bpm)
        {
            Filename = filename;
            BPM = bpm;

            if (!string.IsNullOrEmpty(filename))
            {
                Name = Path.GetFileNameWithoutExtension(filename);
            }

            Length = length.Samples();
            Offset = offset.Samples();

            Pattern = new List<Pattern>();
            Templates = new List<PatternTemplate>();
        }
        public string Name { get; set; }
        public string Filename { get; set; }
        public Sample.Count Length { get; set; }
        public Sample.Count Offset { get; set; }
        public int BPM { get; set; }
        public Sample.Provider Wave { get; set; }
        public Sample.Provider OriginalWave { get; set; }
        public List<Pattern> Pattern { get; private set; }
        public List<PatternTemplate> Templates { get; private set; }
    }
}
