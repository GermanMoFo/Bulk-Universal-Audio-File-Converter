using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using NAudio;
using NAudio.Lame;
using NAudio.Wave;

namespace BUAFC_Library
{
    public static class Conversion
    {
        private static readonly String[] extensions = { "mp3", "wav" };
        public static readonly List<String> SupportedExtensions = new List<String>(extensions);

        public delegate void Converter(string file);

        public static void RunConversions(string[] files, Converter converter, ProgressBar progress)
        {
            //Set Up Progress Bar
            Application.Current.Dispatcher.Invoke(new Action(() => { progress.Maximum = files.Length; }), null);


            foreach (var file in files)
            {
                converter(file);
                Application.Current.Dispatcher.Invoke(new Action(() => { progress.Value++; }), null);
            }
        }

        #region X_To_Wav

        public static void MP3_To_Wav(string mp3file)
        {
            string wavfile = mp3file.Replace(".mp3", ".wav");
            string wavpath = wavfile;

            using (Mp3FileReader reader = new Mp3FileReader(mp3file))
            {

                //step 2: get wave stream with CreatePcmStream method.
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {

                    //step 3: write wave data into file with WaveFileWriter.
                    WaveFileWriter.CreateWaveFile(wavfile, pcmStream);
                }
            }
        }

        #endregion

        #region Wav_To_X


        #endregion
    }
}
