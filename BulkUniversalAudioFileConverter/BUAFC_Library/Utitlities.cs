using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAFC_Library
{
    public static class Utitlities
    {
        public static string FindCommonPath(string Separator, IEnumerable<string> Paths)
        {
            if (Paths.Count() == 0)
                return "";
            if (Paths.Count() == 1)
                return Paths.ToArray()[0];

            string CommonPath = String.Empty;
            List<string> SeparatedPath = Paths
                .First(str => str.Length == Paths.Max(st2 => st2.Length))
                .Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            foreach (string PathSegment in SeparatedPath.AsEnumerable())
            {
                if (CommonPath.Length == 0 && Paths.All(str => str.StartsWith(PathSegment)))
                {
                    CommonPath = PathSegment;
                }
                else if (Paths.All(str => str.StartsWith(CommonPath + Separator + PathSegment)))
                {
                    CommonPath += Separator + PathSegment;
                }
                else
                {
                    break;
                }
            }

            return CommonPath;
        }
    }
}
