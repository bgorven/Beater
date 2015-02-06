using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beater.Models
{
    class Track
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public long Length { get; set; }
        public Sample.Count Offset { get; set; }
        public readonly List<Beat> Pattern = new List<Beat>();
        public readonly List<BeatTemplate> Templates = new List<BeatTemplate>();

        public int BPM { get; set; }

        public Sample.Provider Wave { get; set; }
    }
}
