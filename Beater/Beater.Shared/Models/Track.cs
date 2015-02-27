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
        public Track() : this("Default", null, TimeSpan.FromSeconds(36).Samples(), 0, 60) { }

        public Track(string filename, Sample.Count length, Sample.Count offset, double bpm)
            : this(null, filename, length, offset, bpm)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                Name = Path.GetFileNameWithoutExtension(filename);
            }
        }

        public Track(string name, string filename, Sample.Count length, Sample.Count offset, double bpm)
        {
            Name = name;
            Filename = filename;
            Length = length;
            Offset = offset;
            BPM = bpm;

            Pattern = new List<Pattern>();
            Templates = new List<PatternTemplate>();
        }
        public string Name { get; set; }
        public string Filename { get; set; }
        public Sample.Count Length { get; set; }
        public Sample.Count Offset { get; set; }
        public double BPM { get; set; }
        public Sample.Provider Wave { get; set; }
        public Sample.Provider OriginalWave { get; set; }
        public List<Pattern> Pattern { get; private set; }
        public List<PatternTemplate> Templates { get; private set; }
    }
}
