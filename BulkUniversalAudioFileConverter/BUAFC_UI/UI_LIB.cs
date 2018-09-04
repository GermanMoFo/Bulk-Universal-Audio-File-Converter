using System;
using System.Collections.Generic;
using BUAFC_Library;

namespace BUAFC_UI
{
    static class UI_Lists
    {
        private static readonly String[] extensions = { ".mp3", ".wav" };
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
            string[] flacs_t = { "" };
            List<string> flacs = new List<string>(flacs_t);

            ExtensionUniformer.AddGrouping(".mp3", mp3s);
            ExtensionUniformer.AddGrouping(".wav", wavs);
            ExtensionUniformer.AddGrouping(".flac", flacs);
        }
    }
    
}
