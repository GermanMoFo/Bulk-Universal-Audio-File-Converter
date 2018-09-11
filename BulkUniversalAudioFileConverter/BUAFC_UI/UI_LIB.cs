using System;
using System.Collections.Generic;
using BUAFC_Library;

namespace BUAFC_UI
{
    static class UI_Lists
    {
        private static readonly String[] extensions = { ".mp3", ".wav", ".wma", ".aac", ".ogg" };
        public static readonly List<String> Extensions = new List<String>(extensions);
    }

    public static class UI_LIB
    {
        public static void LoadAlternateExtensions()
        {
            string[] mp3s_t = { "" };
            List<string> mp3s = new List<string>(mp3s_t);
            string[] wavs_t = { ".wave", ".x-wave", ".x-wav" };
            List<string> wavs = new List<string>(wavs_t);
            string[] wmas_t = { ".asf", ".wmv" };
            List<string> wmas = new List<string>(wmas_t);
            string[] aac_t = { ".adts" };
            List<string> aacs = new List<string>(aac_t);
            string[] ogg_T = { "" };
            List<string> oggs = new List<string>(ogg_T);

            //string[] flacs_t = { "" };
            //List<string> flacs = new List<string>(flacs_t);

            ExtensionUniformer.AddGrouping(".mp3", mp3s);
            ExtensionUniformer.AddGrouping(".wav", wavs);
            ExtensionUniformer.AddGrouping(".wma", wmas);
            ExtensionUniformer.AddGrouping(".aac", aacs);
            ExtensionUniformer.AddGrouping(".ogg", oggs);

            //ExtensionUniformer.AddGrouping(".flac", flacs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="numberOfDirectoryLevelsToKeep">0 For No Truncation</param>
        /// <returns></returns>
        public static string TruncatePathToDirectory(string path, int numberOfDirectoryLevelsToKeep)
        {
            if (numberOfDirectoryLevelsToKeep == 0)
                return path;

            List<int> subDirectoryIndexes = new List<int>();

            for (int i = 0; i < path.Length; ++i)
                if (path[i] == '\\')
                    subDirectoryIndexes.Add(i);

            try
            {
                return path.Substring(subDirectoryIndexes[subDirectoryIndexes.Count - numberOfDirectoryLevelsToKeep]);
            }
            catch
            {
                return path;
            }
        }
    }

    
    
}
