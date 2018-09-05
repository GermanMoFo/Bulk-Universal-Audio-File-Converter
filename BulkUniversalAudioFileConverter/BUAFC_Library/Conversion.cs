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
        private static int MP3_Bitrate = 128;


        private static string workingDirectory = Path.GetTempPath();

        public enum PathMode {InPlace, DirectoryDump};
        private static PathMode pathMode = PathMode.InPlace;

        private static readonly String[] extensions = { ".mp3", ".wav" };
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

        public static int MP3_Bitrate1 { get => MP3_Bitrate; set => MP3_Bitrate = value; }

        public static void InitConversionDictionary()
        {
            Extension_WavConversionPair mp3 =  new Extension_WavConversionPair(MP3_To_Wav, Wav_To_MP3);

            ConversionDict.Add(".mp3", mp3);
        }

        #endregion

        public delegate void Update(string CurrentDirectory, string CurrentAction, int Progress);

        /// <summary>
        /// THREAD ENTRY
        /// </summary>
        public static void RunConversions(IEnumerable<string> targetFiles, string destinationExtension, Update update, bool deleteOriginal)
        {
            string currentDirectory = "", currentAction = "";
            int progress = 0;

            //All Converisons Point To WAV
            //This Means That Files That Start Or End As Wavs Must Be Handled Differently Then Others




            foreach (var file in targetFiles)
            {
                //Update Status Strings
                currentDirectory = "Currently working in " + Path.GetDirectoryName(file) + ".";
                currentAction = "Attemping to convert " + Path.GetFileName(file) + " to " + Path.GetFileNameWithoutExtension(file) + destinationExtension + ".";
                callUpdate();

                //Determine The Destination File Path Name Based On PathMode
                string destination = "";

                switch (pathMode)
                {
                    case PathMode.InPlace:
                        //This defines a method wherein the file is left in the directory of the original
                        //Simply drops and changes the extension of the file
                        //This leaves the original and ne in the same directory with the same name
                        //But with differing extensions
                        destination = file.Remove(file.LastIndexOf('.')) + destinationExtension;
                        break;
                    case PathMode.DirectoryDump:
                        //This defines a method wherein all files are dumped into a user-specified directory
                        throw new NotImplementedException("Directory Dump Mode Not Implemented");
                        //break;
                }
                //Determine Procedure Needed Based On Target And Destination Extensions
                if (destinationExtension == ".wav")
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
                progress++;
                currentAction = "Finished converting " + Path.GetFileName(file) + " to " + Path.GetFileNameWithoutExtension(file) + destinationExtension + ".";
                callUpdate();

                if (deleteOriginal)
                {
                    currentAction = "Deleting " + Path.GetFileName(file) + ".";
                    callUpdate();
                    File.Delete(file);
                }
            }

            currentAction = "Finshed All Conversions";
            callUpdate();

            void callUpdate()
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { update(currentDirectory, currentAction, progress); }), null);
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

        public static void FLAC_To_Wav(string flacFile, string wavFile)
        {
            using (NAudio.Flac.FlacReader reader = new NAudio.Flac.FlacReader(flacFile))
            {
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    WaveFileWriter.CreateWaveFile(wavFile, pcmStream);
                }
            }
        }

        public static void OGG_To_Wav(string oggFile, string wavFile)
        {
            
        }

        #endregion

        #region Wav_To_X

        public static void Wav_To_MP3(string waveFile, string mp3File)
        {
            using (var reader = new AudioFileReader(waveFile))
            using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, MP3_Bitrate))
                reader.CopyTo(writer);
        }

        public static void Wav_To_FLAC(string waveFile, string flacFile)
        {
            string tempPath = workingDirectory + Path.GetFileName(waveFile);

            File.Copy(waveFile, tempPath);

            AudioFileReader reader = new AudioFileReader(tempPath);

            //WaveOverFlacStream n = new WaveOverFlacStream(reader, WaveOverFlacStreamMode.Encode);
        }

        #endregion
    }
}
