using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Beater.Models
{
    class Track
    {
        public Track(string filename, TimeSpan length, TimeSpan offset, int BPM)
        {
            Wave = new Sample.Provider(filename);
            this.BPM = BPM;
            Filename = filename;
            Name = Path.GetFileNameWithoutExtension(filename);
            Length = length.Samples();
            Offset = offset.Samples();

            var template = new BeatTemplate(BPM);
            Templates.Add(template);
            var l = Offset + template.Measure;
            while (l < Length)
            {
                Pattern.Add(new Beat(template));
                l += template.Measure;
            }
        }
        public string Name { get; set; }
        public string Filename { get; set; }
        public Sample.Count Length { get; set; }
        public Sample.Count Offset { get; set; }
        public int BPM { get; set; }
        public Sample.Provider Wave { get; set; }

        public readonly List<Beat> Pattern = new List<Beat>();
        public readonly List<BeatTemplate> Templates = new List<BeatTemplate>();
    }
}
