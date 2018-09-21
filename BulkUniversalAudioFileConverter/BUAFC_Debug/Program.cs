using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BUAFC_UI;


namespace BUAFC_Debug
{

    class Program
    {
        static DisplayList tree;
        static string path = @"C:\Users\thepe_000\Desktop\Testing Folder";
        static string userSpecifiedDirectory = @"C:\Users\thepe_000\Documents\Test";
        static string LCD = "";

        static List<string> allPaths = new List<string>();
        static List<string> newPaths = new List<string>();

        private static void Main(string[] args)
        {
            GenerateDisplayList(path);
            LCD = FindCommonPath("\\", allPaths);

            foreach (var path in allPaths)
            {
                newPaths.Add( userSpecifiedDirectory + path.Remove(0, LCD.Length));

                if (Path.HasExtension(path))
                    if (Path.GetExtension(path) == ".txt")
                    {
                        FileInfo fi = new FileInfo(userSpecifiedDirectory + path.Remove(0, LCD.Length));
                        fi.Directory.Create();
                        fi.Create();
                    }
            }

            Console.ReadLine();
        }

        private static void RunTreeDebugger()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path);

            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.IncludeSubdirectories = true;

            //watcher.Changed += new FileSystemEventHandler(watcher_changed);
            watcher.Renamed += new RenamedEventHandler(watcher_renamed);
            watcher.Created += new FileSystemEventHandler(watcher_created);
            watcher.Deleted += new FileSystemEventHandler(watcher_deleted);

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

        private static void watcher_renamed(object sender, RenamedEventArgs e)
        {
            //Triggered By A Renamed File Or Directory,
            //Follow OldPath, update to FullPath
            //And Change Name

            tree = GenerateDisplayList(path);
            Console.Clear();
            tree.Display();

            if (e.FullPath.Contains("MySpecialPhrase"))
            {
                Console.WriteLine("REEEE");
            }

            Console.WriteLine("\n\nAffected File Last Updated: " + e.FullPath);
        }

        private static void watcher_created(object sender, FileSystemEventArgs e)
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

        private static void watcher_deleted(object sender, FileSystemEventArgs e)
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
                allPaths.Add(primaryDirectory.name);

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

                    allPaths.Add(item.name);

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

        public static string FindCommonPath(string Separator, IEnumerable<string> Paths)
        {
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
