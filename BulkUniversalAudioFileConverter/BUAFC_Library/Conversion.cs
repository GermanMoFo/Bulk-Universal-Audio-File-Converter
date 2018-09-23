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
using NAudio.WindowsMediaFormat;
using NAudio.MediaFoundation;
using OggVorbisEncoder;
using NVorbis;

namespace BUAFC_Library
{
    public static class Conversion
    {
        private static int OGG_SAMPLESIZE = 1024;
        private static int _BITRATE = 192000;

        private static string workingDirectory = Path.GetTempPath();

        public enum PathModeType {InPlace, DirectoryDump, SmartDump};

        private static readonly String[] extensions = { ".mp3", ".wav", ".wma", ".aac", ".ogg" };
        public static readonly List<String> SupportedExtensions = new List<String>(extensions);

        public static int OGG_SAMPLESIZE1 { get => OGG_SAMPLESIZE; set => OGG_SAMPLESIZE = value; }
        public static int BitRate { get => _BITRATE; set => _BITRATE = value; }
        public static PathModeType PathMode { get; set; } = PathModeType.InPlace;
        public static string UserSpecifiedDirectory { get; set; }

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

        private static void InitConversionDictionary()
        {
            Extension_WavConversionPair mp3 =  new Extension_WavConversionPair(MP3_To_Wav, Wav_To_MP3);
            Extension_WavConversionPair wma = new Extension_WavConversionPair(WMA_To_Wav, Wav_To_WMA);
            Extension_WavConversionPair aac = new Extension_WavConversionPair(AAC_To_Wav, Wav_To_AAC);
            Extension_WavConversionPair ogg = new Extension_WavConversionPair(Ogg_To_Wav, Wav_To_Ogg);

            ConversionDict.Add(".mp3", mp3);
            ConversionDict.Add(".wma", wma);
            ConversionDict.Add(".aac", aac);
            ConversionDict.Add(".ogg", ogg);
        }

        public static void Initialize()
        {
            InitConversionDictionary();
            //NAudio.MediaFoundation.S
            MediaFoundationInterop.MFStartup(MediaFoundationInterop.MF_VERSION);
            
        }

        #endregion

        public delegate void Update(string CurrentDirectory, string CurrentAction, int Progress, out bool cancel, bool safeToExit);

        /// <summary>
        /// THREAD ENTRY
        /// </summary>
        public static void RunConversions(IEnumerable<string> targetFiles, string destinationExtension, Update update, bool deleteOriginal)
        {
            string currentDirectory = "", currentAction = "";
            int progress = 0;
            bool cancel = false;

            //If the file mode is Smart Dump, Find The Lowest Common Directory
            string LCD = "";
            if(PathMode == PathModeType.SmartDump)
                LCD = Utitlities.FindCommonPath("\\", targetFiles);

            //All Converisons Point Centrally To WAV
            //This Means That Files That Start Or End As Wavs Must Be Handled Differently Then Other
            foreach (var file in targetFiles)
            {
                //Update Status Strings
                currentDirectory = "Currently working in " + Path.GetDirectoryName(file) + ".";
                currentAction = "Attemping to convert " + Path.GetFileName(file) + " to " + Path.GetFileNameWithoutExtension(file) + destinationExtension + ".";
                callUpdate();

                if (cancel)
                {
                    currentAction = "Successfully Terminated.";
                    callUpdate(true);
                    return;
                }

                //Determine The Destination File Path Name Based On PathMode
                string destination = "";

                switch (PathMode)
                {
                    case PathModeType.InPlace:
                        //This defines a method wherein the file is left in the directory of the original
                        //Simply drops and changes the extension of the file
                        //This leaves the original and ne in the same directory with the same name
                        //But with differing extensions
                        destination = file.Remove(file.LastIndexOf('.')) + destinationExtension;
                        break;
                    case PathModeType.DirectoryDump:
                        //This defines a method wherein all files are dumped into a user-specified directory
                        destination = UserSpecifiedDirectory + "\\" + Path.GetFileName(file);
                        break;
                    //break;
                    case PathModeType.SmartDump:
                        //This defines a method wherein all files are put into a user-specified directory
                        //But will also copy the folder structure of the lowest common directory
                        FileInfo fi = new FileInfo(UserSpecifiedDirectory + file.Remove(0, LCD.Length));
                        fi.Directory.Create();
                        destination = fi.FullName;
                        break;
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

                if (cancel)
                {
                    currentAction = "Successfully Terminated.";
                    callUpdate(true);
                    return;
                }

                if (deleteOriginal)
                {
                    currentAction = "Deleting " + Path.GetFileName(file) + ".";
                    callUpdate();
                    File.Delete(file);
                }
            }

            currentAction = "Finshed All Conversions";
            callUpdate();

            if (cancel)
            {
                currentAction = "Successfully Terminated.";
                callUpdate(true);
                return;
            }

            void callUpdate(bool safe = false)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { update(currentDirectory, currentAction, progress, out cancel, safe); }), null);


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

        public static void Ogg_To_Wav(string oggFile, string wavFile)
        {
            using (var reader = new NAudio.Vorbis.VorbisWaveReader(oggFile))
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                    WaveFileWriter.CreateWaveFile(wavFile, pcmStream);
        }

        public static void WMA_To_Wav(string wmaFile, string wavFile)
        {
            using (WMAFileReader reader = new WMAFileReader(wmaFile))
            {

                //step 2: get wave stream with CreatePcmStream method.
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {

                    //step 3: write wave data into file with WaveFileWriter.
                    WaveFileWriter.CreateWaveFile(wavFile, pcmStream);
                }
            }
        }

        public static void AAC_To_Wav(string aacFile, string wavFile)
        {
            using (MediaFoundationReader reader = new MediaFoundationReader(aacFile))
            {
                using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    WaveFileWriter.CreateWaveFile(wavFile, pcmStream);
                }
            }
        }

        #endregion

        #region Wav_To_X

        public static void Wav_To_MP3(string waveFile, string mp3File)
        {
            using (var reader = new AudioFileReader(waveFile))
            using (var writer = new LameMP3FileWriter(mp3File, reader.WaveFormat, BitRate / 1000))
                reader.CopyTo(writer);
        }

        public static void Wav_To_FLAC(string waveFile, string flacFile)
        {
            string tempPath = workingDirectory + Path.GetFileName(waveFile);

            File.Copy(waveFile, tempPath);

            AudioFileReader reader = new AudioFileReader(tempPath);

            //WaveOverFlacStream n = new WaveOverFlacStream(reader, WaveOverFlacStreamMode.Encode);
        }

        public static void Wav_To_WMA(string waveFile, string wmaFile)
        {
            using (var reader = new AudioFileReader(waveFile))
                MediaFoundationEncoder.EncodeToWma(reader, wmaFile, BitRate);
        }

        public static void Wav_To_AAC(string waveFile, string aacFile)
        {
            using (var reader = new AudioFileReader(waveFile))
                MediaFoundationEncoder.EncodeToAac(reader, aacFile, BitRate);
        }

        public static void Wav_To_Ogg(string wavFile, string oggFile)
        {
            File.Create(oggFile).Close();
            var wavstream = new FileStream(wavFile, FileMode.Open, FileAccess.Read);
            var oggstream = new FileStream(oggFile, FileMode.Create, FileAccess.Write);

            // Stores all the static vorbis bitstream settings
            var info = VorbisInfo.InitVariableBitRate(2, 44100, 0.1f);

            // set up our packet->stream encoder
            var serial = new Random().Next();
            var oggStream = new OggStream(serial);

            // =========================================================
            // HEADER
            // =========================================================
            // Vorbis streams begin with three headers; the initial header (with
            // most of the codec setup parameters) which is mandated by the Ogg
            // bitstream spec.  The second header holds any comment fields.  The
            // third header holds the bitstream codebook.
            var headerBuilder = new HeaderPacketBuilder();

            var comments = new Comments();
            comments.AddTag("ARTIST", "TEST");

            var infoPacket = headerBuilder.BuildInfoPacket(info);
            var commentsPacket = headerBuilder.BuildCommentsPacket(comments);
            var booksPacket = headerBuilder.BuildBooksPacket(info);

            oggStream.PacketIn(infoPacket);
            oggStream.PacketIn(commentsPacket);
            oggStream.PacketIn(booksPacket);

            // Flush to force audio data onto its own page per the spec
            OggPage page;
            while (oggStream.PageOut(out page, true))
            {
                oggstream.Write(page.Header, 0, page.Header.Length);
                oggstream.Write(page.Body, 0, page.Body.Length);
            }

            // =========================================================
            // BODY (Audio Data)
            // =========================================================
            var processingState = ProcessingState.Create(info);

            var buffer = new float[info.Channels][];
            buffer[0] = new float[OGG_SAMPLESIZE1];
            buffer[1] = new float[OGG_SAMPLESIZE1];

            var readbuffer = new byte[OGG_SAMPLESIZE1 * 4];
            while (!oggStream.Finished)
            {
                var bytes = wavstream.Read(readbuffer, 0, readbuffer.Length);

                if (bytes == 0)
                {
                    processingState.WriteEndOfStream();
                }
                else
                {
                    var samples = bytes / 4;

                    for (var i = 0; i < samples; i++)
                    {
                        // uninterleave samples
                        buffer[0][i] = (short)((readbuffer[i * 4 + 1] << 8) | (0x00ff & readbuffer[i * 4])) / 32768f;
                        buffer[1][i] = (short)((readbuffer[i * 4 + 3] << 8) | (0x00ff & readbuffer[i * 4 + 2])) / 32768f;
                    }

                    processingState.WriteData(buffer, samples);
                }

                OggPacket packet;
                while (!oggStream.Finished
                       && processingState.PacketOut(out packet))
                {
                    oggStream.PacketIn(packet);

                    while (!oggStream.Finished
                           && oggStream.PageOut(out page, false))
                    {
                        oggstream.Write(page.Header, 0, page.Header.Length);
                        oggstream.Write(page.Body, 0, page.Body.Length);
                    }
                }
            }

            wavstream.Close();
            oggstream.Close();
        }

        #endregion
    }
}
