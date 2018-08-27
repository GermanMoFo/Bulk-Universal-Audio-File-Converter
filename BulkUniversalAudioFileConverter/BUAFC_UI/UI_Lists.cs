using System;
using System.Collections.Generic;

namespace BUAFC_UI
{
    static class UI_Lists
    {
        private static readonly String[] extensions = { "mp3", "wav", "au", "aiff"};
        public static readonly List<String> Extensions = new List<String>(extensions);
    }
}
