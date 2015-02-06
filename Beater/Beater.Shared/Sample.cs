using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Beater
{
    static class Sample
    {
        public const long SampleRateHz = 44100;

        public struct Count
        {
            public readonly long Value;

            public Count(long val)
            {
                this.Value = val;
            }

            public static implicit operator long(Count val)
            {
                return val.Value;
            }

            public static implicit operator Count(long val)
            {
                return new Count(val);
            }

            public TimeSpan Time(){
                return checked(TimeSpan.FromTicks(Value * TimeSpan.TicksPerSecond / Sample.SampleRateHz));
            }
        }

        public static Count Samples(this TimeSpan val){
            return checked(val.Ticks * SampleRateHz / TimeSpan.TicksPerSecond);
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

            public Provider(Provider copy)
            {
                Samples = copy.Samples;
                WaveFormat = copy.WaveFormat;
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
        }
    }
}
