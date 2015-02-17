using Beater.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beater.Audio
{
    class TrackSampleProvider
    {
        private TrackViewModel _track;

        public TrackSampleProvider(TrackViewModel track)
        {
            _track = track;
        }

        internal void MixTo(Sample.Count start, float[] buffer, int offset, int count)
        {
            int location = 0, end = start + count;
            foreach (var pattern in _track.Pattern)
            {
                var patternEnd = location + pattern.Measure * 2;
                if (patternEnd > start)
                {
                    foreach (var beat in pattern.Beats)
                    {
                        if (beat.Beats)
                        {
                            if (location >= start)
                            {
                                _track.Wave.MixTo(location - start, buffer, offset, count);
                            }
                            else
                            {
                                _track.Wave.MixTo(0, buffer, offset + (start - location), count - (start - location));
                            }
                        }
                        location += pattern.BeatLength * 2;
                        if (location >= end) return;
                    }
                }
                location = patternEnd;
                if (location >= end) return;
            }
        }
    }
}
