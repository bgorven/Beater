using System;
using System.Collections.Generic;
using System.Text;

namespace Beater.Models
{
    class Beat
    {
        public Sample.Count BeatLength { get; set; }
        public Sample.Count Measure { get; set; }

        public float SpeedMultiplier = 1;

        public Sample.Provider Original { get; set; }

        public bool[] Beats { get; set; }

        public string Id { get; set; }
    }
}
