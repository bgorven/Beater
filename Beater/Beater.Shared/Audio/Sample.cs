using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Beater.Audio
{
    static class Sample
    {
        /// <summary>
        /// Raw number of samples per second (base sample rate * number of channels)
        /// </summary>
        public const int SamplesPerSecond = 88200;

        public static readonly WaveFormat WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

        public struct Count
        {
            public readonly int Value;

            public Count(int val)
            {
                this.Value = val;
            }

            public static implicit operator int(Count val)
            {
                return val.Value;
            }

            public static implicit operator Count(int val)
            {
                return new Count(val);
            }

            public TimeSpan Time(){
                return checked(TimeSpan.FromTicks(Value * TimeSpan.TicksPerSecond / Sample.SamplesPerSecond));
            }

            public bool Equals(Count other)
            {
                return this.Value == other.Value;
            }

            public override string ToString()
            {
                return Value.ToString() + " samples";
            }

            public override bool Equals(object obj)
            {
                if (obj is Count)
                {
                    return this.Equals((Count)obj);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        public static Count Samples(this TimeSpan val){
            return checked((int)(val.Ticks * SamplesPerSecond / TimeSpan.TicksPerSecond));
        }

        public class Provider : ISampleProvider
        {
            private int position = 0;
            public readonly float[] Samples;

            public Provider(string filename)
            {
                using (var reader = new MediaFoundationReader(filename))
                {
                    var sampleProvider = reader.ToSampleProvider();
                    var count = reader.Length / (reader.WaveFormat.BitsPerSample / 8);
                    Samples = new float[count];
                    WaveFormat = reader.WaveFormat;
                    var numRead = sampleProvider.Read(Samples, 0, (int)count);
                    if (numRead != count) throw new InvalidOperationException("Reading ‘" + filename + "’ failed unexpectedly.");
                }
            }

            public Provider(float[] samples, WaveFormat format)
            {
                Samples = samples;
                WaveFormat = format;
            }

            public WaveFormat WaveFormat { get; private set; }

            public int Read(float[] buffer, int offset, int count)
            {
                var available = Samples.Length - position;
                count = count > available ? available : count;
                Array.Copy(Samples, position, buffer, offset, count);
                position += count;
                return count;
            }

            public void Reset()
            {
                position = 0;
            }

            internal void MixTo(int start, float[] buffer, int offset, int count)
            {
                for (int i = start;
                    i < start + count && i < Samples.Length && offset < buffer.Length;
                    i++, offset++)
                {
                    buffer[offset] += Samples[i];
                }
            }
        }
    }
}
