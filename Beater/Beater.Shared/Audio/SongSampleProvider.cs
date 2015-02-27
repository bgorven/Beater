using Beater.ViewModels;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beater.Audio
{
    class SongSampleProvider : ISampleProvider
    {
        private SongViewModel _song;

        public SongSampleProvider(SongViewModel song)
        {
            _song = song;
        }

        public WaveFormat WaveFormat
        {
            get { return Sample.WaveFormat; }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (var i = offset; i < offset + count && i < buffer.Length; i++) buffer[i] = 0;

            var countToEnd = Math.Min(count, _song.Length - _song.Progress);
            var countFromStart = count - countToEnd;

            MixTo(_song.Progress, buffer, offset, countToEnd);
            if (countFromStart > 0)
            {
                MixTo(0, buffer, offset + countToEnd, countFromStart);
                _song.SetProgress(countFromStart);
            }
            else
            {
                _song.SetProgress(_song.Progress + count);
            }
            return count;
        }

        private void MixTo(int bufStart, float[] buffer, int offset, int count)
        {
            foreach (var track in _song.Tracks)
            {
                track.Provider.MixTo(bufStart, buffer, offset, count);
            }
        }
    }
}
