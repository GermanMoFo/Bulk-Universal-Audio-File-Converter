using System;
using System.Collections.Generic;

namespace BUAFC_UI\
{
    static class PROTO_Lists
    {
        private static readonly String[] extensions = { ".FLAC", ".MP3", ".WAV", ".OGG", ".GSM", "DCT", ".AU", ".VOX", ".WMA", ".AAC", ".RA", ".RAM", ".RA", ".DSS", ".MSV", ".DVF", ".ATRAC", ".MID", ".APE"};
        public static readonly List<String> Extensions = new List<String>(extensions);
    }
}
