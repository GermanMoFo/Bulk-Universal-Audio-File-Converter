using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAFC_Library
{
    public struct ExtensionGrouping
    {
        public string PrimaryExtension;
        public List<string> AlternateExtensions;

        public ExtensionGrouping(string primaryExtension, List<string> alternateExtensions)
        {
            PrimaryExtension = primaryExtension;
            AlternateExtensions = alternateExtensions;
        }
    }

    public static class ExtensionUniformer
    {
        static List<ExtensionGrouping> extensionGroupings = new List<ExtensionGrouping>();

        static public void AddGrouping(string Primary, IEnumerable<string> Alternates)
        {

            List<string> temp = new List<string>();

            foreach (string s in Alternates)
                temp.Add(s.ToLower());

            extensionGroupings.Add(new ExtensionGrouping(Primary.ToLower(), temp));
        }

        /// <summary>
        /// Returns Null if Not Supported
        /// </summary>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static string UnifyExtension(string Extension)
        {
            Extension = Extension.ToLower();

            foreach (var grouping in extensionGroupings)
            {
                if (grouping.PrimaryExtension == Extension)
                    return grouping.PrimaryExtension;

                else
                    foreach (var s in grouping.AlternateExtensions)
                        if (s == Extension)
                            return grouping.PrimaryExtension;
            }

            return null;
        }
    }
}
