using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beater.Models
{
    class Track
    {
        public Sample.Count Offset { get; set; }
        public Beat[] Templates { get; set; }
        public List<Beat> Pattern = new List<Beat>();
    }
}
