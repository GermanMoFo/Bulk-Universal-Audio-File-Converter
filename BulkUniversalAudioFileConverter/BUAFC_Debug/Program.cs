using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NVorbis;
using NAudio;
using NAudio.Wave;
using NAudio.Vorbis;
using OggVorbisEncoder;

namespace BUAFC_Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            string workingDirectory = @"C:\Users\thepe_000\Music\working";
            string waveFile = @"C:\Users\thepe_000\Music\Alan Parsons_702 Rocker\Loop\Beats\DrumLoop1_121.5bpm.wav";
            string flacFile = @"C:\Users\thepe_000\Music\Alan Parsons_702 Rocker\Loop\Beats\DrumLoop1_121.5bpm.flac";
            string oggFile = @"C:\Users\thepe_000\Music\Alan Parsons_702 Rocker\Loop\Beats\DrumLoop1_121.5bpm.ogg";

            using (var reader = new AudioFileReader(waveFile))
            {
                using (var writer = new OggVorbisEncoder.Enco)
            }
        }
    }
}
