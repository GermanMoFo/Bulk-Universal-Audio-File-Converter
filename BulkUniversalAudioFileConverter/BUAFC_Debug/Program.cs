using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;
using NAudio.WindowsMediaFormat;
using NAudio;
using OggVorbisEncoder;

namespace BUAFC_Debug
{

    class Program
    {
        private static void Main(string[] args)
        {
            string hello = @"C:\Users\Joshua\Desktop\Bulk-Universal-Audio-File-Converter\BulkUniversalAudioFileConverter\AFile.txt";

            string temp = TruncatePathToDirectory(hello, 2);

            Console.WriteLine("..." + temp);

            Console.ReadLine();
        }

        public static string TruncatePathToDirectory(string path, int numberOfDirectoryLevelsToKeep)
        {
            List<int> subDirectoryIndexes = new List<int>();

            for (int i = 0; i < path.Length; ++i)
                if (path[i] == '\\')
                    subDirectoryIndexes.Add(i);

            return path.Substring(subDirectoryIndexes[subDirectoryIndexes.Count - numberOfDirectoryLevelsToKeep]);
        }
    }
}
