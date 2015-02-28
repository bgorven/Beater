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

        internal void MixTo(int bufStart, float[] buffer, int offset, int count)
        {
            int location = 0, bufEnd = bufStart + count;
            foreach (var pattern in _track.Pattern)
            {
                var patternEnd = location + pattern.Measure;
                foreach (var beat in pattern.Beats)
                {
                    if (beat.Active)
                    {
                        var startOffset = location - bufStart;
                        var samplesAvailable = _track.Wave.Samples.Length - startOffset;
                        var readEnd = (bufStart + Math.Min(count, samplesAvailable));

                        if (startOffset < 0)
                        {
                            //beat starts before buffer, begin read -startOffset samples into wave.
                            if (samplesAvailable > 0)
                            {
                                _track.Wave.MixTo(-startOffset, buffer, offset, Math.Min(count, samplesAvailable));
                            }
                        }
                        else if (count - startOffset > 0)
                        {
                            //beat starts after buffer, place read +startOffset samples into buffer.
                            _track.Wave.MixTo(0, buffer, offset + startOffset, count - startOffset);
                        }
                        else
                        {
                            var beatStart = location;
                            var beatEnd = beatStart + _track.Wave.Samples.Length;
                            if (beatStart < bufEnd && beatEnd > bufStart)
                            {
                                ;
                            }
                        }

                    }
                    else
                    {
                        ;
                    }
                    location += pattern.BeatLength;
                    if (location >= bufEnd)
                    {
                        return;
                    }
                }

                location = patternEnd;
                if (location >= bufEnd) return;
            }
        }
    }
}
