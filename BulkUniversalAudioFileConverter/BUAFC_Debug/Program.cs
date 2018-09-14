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
        static DisplayList tree;
        static string path = @"C:\Users\thepe_000\Desktop\Testing Folder";
        private static void Main(string[] args)
        {

            

            FileSystemWatcher watcher = new FileSystemWatcher(path);

            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.Size;
            watcher.IncludeSubdirectories = true;
            watcher.Changed += new FileSystemEventHandler(watcher_changed);
            watcher.Renamed += new RenamedEventHandler(watcher_changed);
            tree = GenerateDisplayList(path);
            tree.Display();




            Console.ReadLine();
        }

        private static void watcher_changed(object sender, FileSystemEventArgs e)
        {
            tree = GenerateDisplayList(path);
            Console.Clear();
            tree.Display();

            if (e.FullPath.Contains("MySpecialPhrase"))
            {
                Console.WriteLine("REEEE");
            }

            Console.WriteLine("\n\nAffected File Last Updated: " + e.FullPath);
        }

        public static DisplayList GenerateDisplayList(string path)
        {
            DisplayList displayList = new DisplayList(path);
            displayList.children = new List<DisplayList>();

            //Iterates all primary sub-directories of given path
            foreach (string folder in Directory.EnumerateFileSystemEntries(path))
            {
                //Generate Tree View Node For Each Directory
                DisplayList primaryDirectory = new DisplayList(folder);


                if (!Path.HasExtension(folder))
                    RecursiveCatalog(folder, primaryDirectory);

                RecursiveTabbing(primaryDirectory);

                //Add the node to the tree
                displayList.children.Add(primaryDirectory);
            }

            //_______________________________------------------------_______________________________//

            return displayList;

            void RecursiveCatalog(string _path, DisplayList parent)
            {
                parent.children = new List<DisplayList>();

                foreach (string str in Directory.EnumerateFileSystemEntries(_path))
                {
                    //Generate Tree View Node For Each Directory
                    DisplayList item = new DisplayList(str);

                    //Add node to parent's descendants
                    parent.children.Add(item);

                    //Recurse to find additional sub-directories
                    if (!Path.HasExtension(str))
                        RecursiveCatalog(str, item);

                    RecursiveTabbing(item);
                }
            }

            void RecursiveTabbing(DisplayList parent)
            {
                if (parent.children != null)
                    foreach (var child in parent.children)
                    {
                        child.name = '\t' + child.name;

                        RecursiveTabbing(child);
                    }
            }
        }

        public class DisplayList
        {
            public string name;
            public List<DisplayList> children;

            public DisplayList(string _name)
            {
                name = _name;
            }

            public void Display()
            {
                Console.WriteLine(name);

                if (children != null)
                    foreach (var child in children)
                    {
                        child.name = '\t' + child.name;
                        child.Display();
                    }
            }
        }
    }
}
