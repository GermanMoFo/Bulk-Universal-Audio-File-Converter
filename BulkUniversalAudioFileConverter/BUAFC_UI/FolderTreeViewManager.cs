using BUAFC_Library;
using CheckBoxTreeView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BUAFC_UI
{
    /// <summary>
    /// Class Meant To Abstract All Tree View Handlings From Main Form
    /// </summary>
    static class FolderTreeViewManager
    {

        // Member Variables
        static TreeView Tree;
        static String BaseDirectory;
        static System.ComponentModel.PropertyChangedEventHandler CheckBoxChanged;
        static FileSystemWatcher Watcher;

        static DoubleAssociativeList<string, TreeViewModel> nodeList = new DoubleAssociativeList<string, TreeViewModel>();

        public static DoubleAssociativeList<string, TreeViewModel> NodeList
        {
            get
            {
                return nodeList;
            }
        }


        /// <summary>
        /// Sets the watcher library to use passed directory, fills in the tree view
        /// 
        /// Does not clear TreeView
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="baseDirectory"></param>
        /// <param name="checkBoxChanged"></param>
        public static void Initialize(TreeView tree, string baseDirectory, System.ComponentModel.PropertyChangedEventHandler checkBoxChanged, Action notifyFinished)
        {
            Tree = tree;
            BaseDirectory = baseDirectory;
            CheckBoxChanged = checkBoxChanged;

            GenerateTreeView(Tree, BaseDirectory);

            InitializeFileSystemWatcher(BaseDirectory, Watcher);
            
            Application.Current.Dispatcher.Invoke(notifyFinished, null);
        }

        #region View Generation

        private static void GenerateTreeView(TreeView treeView, string path)
        {
            //Create A Temporary Item List
            List<TreeViewModel> items = new List<TreeViewModel>();

            //Iterates all primary sub-directories of given path
            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                //Generate Tree View Node For Each Directory
                TreeViewModel primaryDirectory = new TreeViewModel();
                primaryDirectory.Name = folder.Substring(folder.LastIndexOf('\\') + 1);
                primaryDirectory.Tag = folder;

                //Track Node  By Adding It List
                nodeList.Add(folder, primaryDirectory);

                //Generate the folders Sub-Directory Nodes
                FillTreeView(primaryDirectory, folder);

                //Initialize Node
                primaryDirectory.Initialize();

                //Add the node to the tree
                //Application.Current.Dispatcher.Invoke(new Action(() => { treeView.Items.Add(primaryDirectory); }), null);
                items.Add(primaryDirectory);
                
            }

            //Assign The Original Collection To The New One
            Application.Current.Dispatcher.Invoke(new Action(() => { treeView.ItemsSource = items; }), null);
        }

        private static void FillTreeView(TreeViewModel parentItem, string path)
        {
            foreach (string str in Directory.EnumerateDirectories(path))
            {
                //Generate Tree View Node For Each Directory
                TreeViewModel item = new TreeViewModel();
                item.Name = str.Substring(str.LastIndexOf('\\') + 1);
                item.Tag = str;

                //Track Node  By Adding It List
                nodeList.Add(str, item);

                //Add node to parent's descendants
                parentItem.Children.Add(item);

                //Recurse to find additional sub-directories
                FillTreeView(item, str);

            }

            //Catalog All Files As Check Boxes
            foreach (string file in Directory.EnumerateFiles(path))
            {
                FileInfo fi = new FileInfo(file);
            
                //Check to ensure the file type is an acceptable one
                if (ExtensionUniformer.UnifyExtension(fi.Extension) == null)
                    continue;
            
                TreeViewModel item_file = new TreeViewModel();
            
                item_file.Name = fi.Name;
                item_file.Tag = file;
            
                item_file.PropertyChanged += CheckBoxChanged;
            
                parentItem.Children.Add(item_file);

                //Track Node  By Adding It List
                nodeList.Add(file, item_file);

                item_file.Initialize();
            }

        }

        #endregion

        #region FileSystemWatcherHandlings

        private static void InitializeFileSystemWatcher(string path, FileSystemWatcher watcher)
        {
            watcher = new FileSystemWatcher(path);

            //Set Up Properties
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

            //Set Up Events
            watcher.Renamed += new RenamedEventHandler(watcher_renamed);
            watcher.Created += new FileSystemEventHandler(watcher_created);
            watcher.Deleted += new FileSystemEventHandler(watcher_deleted);
        }

        private static void watcher_renamed(object sender, RenamedEventArgs e)
        {
            //Find Corresponding Tree Node
            TreeViewModel model = nodeList[e.OldFullPath];

            //Update Meta-Data
            model.Tag = e.FullPath;

            //Update Display Name
            model.Name = Path.GetFileName(e.FullPath);

            //Refresh Items So It Reflects Change
            Application.Current.Dispatcher.Invoke(new Action(() => { Tree.Items.Refresh(); }), null);
        }

        private static void watcher_created(object sender, FileSystemEventArgs e)
        {
            //Triggered When A File Is Created
            //Follow Tree Down By Path, create new TreeViewModel
            //e.fullPath is the name off the created file

            //folders with multiple entries?
            //1st folder created closest to root directory
            //2nd items within that folder
            //3rd items within those folders

            //More than one folder within a sudir?
            //enters each directory after calling them

            //Just create a new node at the loation sent
            //An entry will never be reported b4 its
            //supporting directories

            //--------------------------------//

            //Find the FolderTreeViewModel for which the sent entry exists in
            TreeViewModel dir = nodeList[e.FullPath.Remove(e.FullPath.LastIndexOf('\\'))];

            //Create A New FolderTreeViewModel to represent the sent entry
            TreeViewModel entry = new TreeViewModel();
            
            //Generate its name based on whether it is a directory or a file
            if (Path.HasExtension(e.FullPath))
            {
                //Is A File
                entry.Name = Path.GetFileName(e.FullPath);
            }
            else
            {
                //Is Not A File
                entry.Name = Path.GetDirectoryName(e.FullPath);
            }

            //Add New FolderTreeViewModel to its parent's chilren
            dir.Children.Add(entry);

            //Initialize this node by assigning its parent.
            entry.Parent = dir;

            //Refresh Items So It Reflects Change
            Application.Current.Dispatcher.Invoke(new Action(() => { Tree.Items.Refresh(); }), null);
        }

        private static void watcher_deleted(object sender, FileSystemEventArgs e)
        {
            //Triggered by a deleted file,
            //e.fullPath is the name of the file
            //Find file and delete

            //?? what will happen if a whole folder with sud-dirs and files is deleted?
            //Only the highest-level folder is reported
            //this means you only need to delete the corresponding node

            //--------------------------------//

            //Find the FolderTreeViewModel for which the sent entry exists in
            TreeViewModel dir = nodeList[e.FullPath.Remove(e.FullPath.LastIndexOf('\\'))];

            //Find the FolderTreeViewModel for the sent entry
            TreeViewModel deleted = nodeList[e.FullPath];

            //Remove the deleted node from the children of its directory
            dir.Children.Remove(deleted);

            //Refresh Items So It Reflects Change
            Application.Current.Dispatcher.Invoke(new Action(() => { Tree.Items.Refresh(); }), null);
        }

        #endregion
    }
}
