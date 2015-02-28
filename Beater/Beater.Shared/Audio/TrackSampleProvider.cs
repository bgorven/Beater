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
            //The time into the track that the output buffer ends
            var bufEnd = bufStart + count;

            foreach (var pattern in _track.Pattern)
            {
                var patternStart = pattern.Location;
                var patternEnd = patternStart + pattern.Measure;

                if (patternStart >= bufEnd)
                {
                    break;
                }

                foreach (var beat in pattern.Beats)
                {
                    if (beat.Active)
                    {
                        var beatStart = patternStart + beat.Location;
                        var startOffset = beatStart - bufStart;
                        var samplesAvailable = _track.Wave.Samples.Length - startOffset;
                        var readEnd = (bufStart + Math.Min(count, samplesAvailable));

                        if (beatStart >= bufEnd)
                        {
                            break;
                        }

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

                    }
                }
            }
        }
    }
}
