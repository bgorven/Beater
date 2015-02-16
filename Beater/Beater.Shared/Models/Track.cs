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
            if (!string.IsNullOrEmpty(filename))
            {
                Wave = new Sample.Provider(filename);
                Name = Path.GetFileNameWithoutExtension(filename);
                Filename = filename;
            }
            else
            {
                Name = "Untitled";
            }

            this.BPM = BPM;
            Length = length.Samples();
            Offset = offset.Samples();

            Pattern = new List<Pattern>();
            Templates = new List<PatternTemplate>();

            var template = new PatternTemplate(BPM);
            Templates.Add(template);
            var l = Offset + template.Measure;
            while (l < Length)
            {
                Pattern.Add(new Pattern(template));
                l += template.Measure;
            }
        }
        public string Name { get; set; }
        public string Filename { get; set; }
        public Sample.Count Length { get; set; }
        public Sample.Count Offset { get; set; }
        public int BPM { get; set; }
        public Sample.Provider Wave { get; set; }

        public List<Pattern> Pattern { get; private set; }
        public List<PatternTemplate> Templates { get; private set; }
    }
}
