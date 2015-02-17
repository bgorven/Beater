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
            count = Math.Min(count, _song.SampleCount - _song.Progress * 2);
            if (count <= 0) return 0;

            for (var i = offset; i < offset + count && i < buffer.Length; i++) buffer[i] = 0;

            foreach (var track in _song.Tracks)
            {
                track.Provider.MixTo(_song.Progress * 2, buffer, offset, count);
            }

            _song.SetProgress(_song.Progress + count / 2);

            return count;
        }
    }
}
