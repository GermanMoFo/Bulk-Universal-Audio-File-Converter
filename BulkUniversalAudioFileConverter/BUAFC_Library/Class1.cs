using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbingersys.Audio.Aumplib;

namespace BUAFC_Library
{
    public class Class1
    {
        private Aumpel audioConverter = new Aumpel();
        private Aumpel.soundFormat inputFileFormat;
        private Aumpel.soundFormat outputFileFormat;

        private static int soundFileSize;

        private static string[] fileExtensions = { "WAV", "MP3", "AU", "AIFF" };
        public readonly List<String> SupportedFileExtensions = new List<String>(fileExtensions);

        // Conversion callback (lame,libsndfile)
        public static void ReportStatus(int totalBytes, int processedBytes, Aumpel aumpelObj, ref double progress)
        {
            progress = (int)(((float)processedBytes / (float)totalBytes) * 100);
        }

        // Decoding callback (madlldlib)
        public static bool ReportStatusMad(uint frameCount, uint byteCount, ref MadlldlibWrapper.mad_header mh, ref double progress)
        {
            progress = (int)(((float)byteCount / (float)soundFileSize) * 100);
            return true;
        }

        public bool SetOutPutFileFormat(string format)
        {
            if (SupportedFileExtensions.Contains(format))
            {
                outputFileFormat = StringToSoundFormat(format);
                return true;
            }

            return false;
        }

        public bool SetInPutFileFormat(string file)
        {
            try
            {
                inputFileFormat = audioConverter.CheckSoundFormat(file);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private Aumpel.soundFormat StringToSoundFormat(string Extension)
        {
            switch (Extension)
            {
                case "WAV":
                    return Aumpel.soundFormat.WAV;
                case "MP3":
                    return Aumpel.soundFormat.MP3;
                case "AU":
                    return Aumpel.soundFormat.AU;
                case "AIFF":
                    return Aumpel.soundFormat.AIFF;
            }

            throw new ArgumentException("File extension: " + Extension + " is not supported.");
        }

        private bool Convert(string inputFile, string outputFile, ref double progress)
        {
            if ((int)outputFileFormat == (int)Aumpel.soundFormat.MP3) return ConvertToMP3(inputFile, outputFile, ref progress);
            else if ((int)inputFileFormat == (int)Aumpel.soundFormat.MP3) return ConvertFromMP3(inputFile, outputFile);
            else return NonMP3Conversion(inputFile, outputFile, ref progress);
        }


        private bool ConvertToMP3(string inputFile, string outputFile, ref double progress)
        {
            if ((int)outputFileFormat == (int)Aumpel.soundFormat.MP3)
            {

                try
                {
                    Aumpel.Reporter defaultCallback = new Aumpel.Reporter(ReportStatus);

                    audioConverter.Convert(inputFile, (int)inputFileFormat, outputFile, (int)outputFileFormat, defaultCallback, ref progress);

                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private bool ConvertFromMP3(string inputFile, string outputFile)
        {
            try
            {
                MadlldlibWrapper.Callback defaultCallback = new MadlldlibWrapper.Callback(ReportStatusMad);

                // Determine file size
                FileInfo fi = new FileInfo(inputFile);
                soundFileSize = (int)fi.Length;

                audioConverter.Convert(inputFile, outputFile, outputFileFormat, defaultCallback);
                
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool NonMP3Conversion(string inputFile, string outputFile, ref double progress)
        {
            try
            {
                Aumpel.Reporter defaultCallback = new Aumpel.Reporter(ReportStatus);

                audioConverter.Convert(inputFile, (int)inputFileFormat, outputFile, (int)(outputFileFormat | Aumpel.soundFormat.PCM_16), defaultCallback, ref progress);

            }
            catch
            {
                return false;
            }


            return true;
        }
    }
}
