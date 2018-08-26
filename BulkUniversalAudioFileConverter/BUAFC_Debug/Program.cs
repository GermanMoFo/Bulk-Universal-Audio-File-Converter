using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace BUAFC_Debug
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = "F:\\Music\\Electronic";

            foreach (var file in Directory.EnumerateFiles(dir))
                Console.WriteLine(file);

            Console.ReadLine();

        }
    }
}
