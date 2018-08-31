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
        private static int MP3_Bitrate;


        private static string workingDirectory = Path.GetTempPath();

        public enum PathMode {InPlace, DirectoryDump};
        private static PathMode pathMode = PathMode.InPlace;

        private static readonly String[] extensions = { "mp3", "wav" };
        public static readonly List<String> SupportedExtensions = new List<String>(extensions);

        #region Extension-To-Conversion Method Association

        /// <summary>
        /// The goal of the following code segments is to create a dictionary 
        /// that associates string extensions with the supported file extensions
        /// 
        /// This is done by creating a pair of delegate and inserting them into a struct,
        /// Then entering this struct into a dictionary where it is indexed by its string file extension
        /// 
        /// Doing this allows for better code flow and readability
        /// 
        /// Here variables/Field that mention X refer to 
        /// </summary>
        /// 

        private delegate void XToWav(string xFilePath, string wavFilePath);
        private delegate void WavToX(string wavFilePath, string xFilePath);

        private struct Extension_WavConversionPair
        {
            public XToWav XToWav;
            public WavToX WavToX;

            public Extension_WavConversionPair(XToWav xToWav, WavToX wavToX)
            {
                XToWav = xToWav;
                WavToX = wavToX;
            }
        }

        private static Dictionary<string, Extension_WavConversionPair> ConversionDict = new Dictionary<string, Extension_WavConversionPair>();

        public static void InitConversionDictionary()
        {
            Extension_WavConversionPair mp3 =  new Extension_WavConversionPair(MP3_To_Wav, Wav_To_MP3);

            ConversionDict.Add("mp3", mp3);
        }

        #endregion

        /// <summary>
        /// THREAD ENTRY
        /// </summary>
        /// <param name="targetFiles"></param>
        /// <param name="destinationExtension"></param>
        /// <param name="progress"></param>
        public static void RunConversions(IEnumerable<string> targetFiles, string destinationExtension, ProgressBar progress)
        {
            //Set Up Progress Bar
            Application.Current.Dispatcher.Invoke(new Action(() => { progress.Maximum = targetFiles.Count(); }), null);

            //All Converisons Point To WAV
            //This Means That Files That Start Or End As Wavs Must Be Handled Differently Then Others


            foreach (var file in targetFiles)
            {
                //Determine The Destination File Path Name Based On PathMode
                string destination = "";

                switch (pathMode)
                {
                    case PathMode.InPlace:
                        //This defines a method wherein the file is left in the directory of the original
                        //Simply drops and changes the extension of the file
                        //This leaves the original and ne in the same directory with the same name
                        //But with differing extensions
                        destination = file.Remove(file.LastIndexOf('.') + 1) + destinationExtension;
                        break;
                    case PathMode.DirectoryDump:
                        //This defines a method wherein all files are dumped into a user-specified directory
                        throw new NotImplementedException("Directory Dump Mode Not Implemented");
                        //break;
                }
                //Determine Procedure Needed Based On Target And Destination Extensions
                if (destinationExtension == "wav")
                {
                    //X_TO_WAV One Way

                    //Find Correct Converter Based On Target Extension
                    XToWav toWav = ConversionDict[Path.GetExtension(file)].XToWav;

                    //Convert
                    toWav(file, destination);
                }
                else if(Path.GetExtension(file) == ".wav")
                {
                    //WAV_TO_X One Way

                    //Find Correct Converter Based Destination Extension
                    WavToX toX = ConversionDict[destinationExtension].WavToX;

                    //Convert
                    toX(file, destination);

                }
                else
                {
                    //X_TO_X Two Way

                    //Get Both Convters
                    XToWav toWav = ConversionDict[Path.GetExtension(file)].XToWav;
                    WavToX toX = ConversionDict[destinationExtension].WavToX;

                    //Create The Path For The Temporary Wav File
                    string tempPath = workingDirectory + Path.GetFileNameWithoutExtension(file) + ".wav";

                    //Convert To a Temp Wav
                    toWav(file, tempPath);

                    //Convert Temp Wav File To X
                    toX(tempPath, destination);

                    //Delete Temp File
                    File.Delete(tempPath);
                }


                //Update Progress Bar
                Application.Current.Dispatcher.Invoke(new Action(() => { progress.Value++; }), null);
            }
        }

        #region X_To_Wav

        public static void MP3_To_Wav(string mp3File, string wavFile)
        {

            using (Mp3FileReader reader = new Mp3FileReader(mp3File))
            {

                //step 2: get wave stream with CreatePcmStream method.
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {

                    //step 3: write wave data into file with WaveFileWriter.
                    WaveFileWriter.CreateWaveFile(wavFile, pcmStream);
                }
            }
        }

        #endregion

        #region Wav_To_X

        public static void Wav_To_MP3(string waveFile, string mp3File)
        {
            using (var reader = new AudioFileReader(waveFile))
            using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, MP3_Bitrate))
                reader.CopyTo(writer);
        }

        #endregion
    }
}
