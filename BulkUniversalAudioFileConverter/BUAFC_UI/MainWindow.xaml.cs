using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using BUAFC_Library;
using CheckBoxTreeView;
using System.ComponentModel;

namespace BUAFC_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> SelectedFiles = new List<string>();
        bool deleteOriginals = false;

        public MainWindow()
        {
            InitializeComponent();


            //Input Supported File Types Into Drop Downs
            CMBB_FROM.Items.Add("ALL...");
            CMBB_FROM.SelectedIndex = 0;
            PopulateItemCollectionFromIEnurmable(CMBB_FROM.Items, Conversion.SupportedExtensions);
            PopulateItemCollectionFromIEnurmable(CMBB_AS.Items,   Conversion.SupportedExtensions);
            PopulateItemCollectionFromIEnurmable(CMBB_TO.Items,   Conversion.SupportedExtensions);

            //Generate Extension Unifiers To Accomodate Weird Extensions
            UI_LIB.LoadAlternateExtensions();

            //Generate Conversion Method Library
            Conversion.InitConversionDictionary();

            //Populate Tree Views
            //GenerateTreeView(TV_FROM, @"F:\Music");
            //GenerateTreeView(TV_FROM, @"C:\Users\Joshua\Desktop\Seether");
            GenerateTreeView(TV_FROM, @"C:\Users\thepe_000\Music");

            //TV_FROM.ItemsSource = TreeViewModel.SetTree("Top Level");

            LB_CHECK.ItemsSource = SelectedFiles;

            
        }

        

        #region EventHandlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if(radioButton != null)
            {
                if (radioButton.Name == "RB_CUSTOM")
                {
                    if (CMBB_FROM != null)          CMBB_FROM.IsEnabled =           false;
                    if (TXTBX_RECKOGNIZE != null)   TXTBX_RECKOGNIZE.IsEnabled =    true;
                    if (LB_RECKOGNIZE != null)      LB_RECKOGNIZE.IsEnabled =       true;
                    if (LB_AS != null)              LB_AS.IsEnabled =               true;
                    if (CMBB_AS != null)            CMBB_AS.IsEnabled =             true;

                }

                if (radioButton.Name == "RB_STRICT")
                {
                    if (CMBB_FROM != null)          CMBB_FROM.IsEnabled =           true;
                    if (TXTBX_RECKOGNIZE != null)   TXTBX_RECKOGNIZE.IsEnabled =    false;
                    if (LB_RECKOGNIZE != null)      LB_RECKOGNIZE.IsEnabled =       false;
                    if (LB_AS != null)              LB_AS.IsEnabled =               false;
                    if (CMBB_AS != null)            CMBB_AS.IsEnabled =             false;
                }
            }
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {

            //VERIFY ENOUGH OPTIONS ARE SET
            bool check = true;
            //FROM
            if (RB_STRICT.IsChecked == true && CMBB_FROM.SelectedItem == null)
                check = false;
            else if (RB_CUSTOM.IsChecked == true && CMBB_AS.SelectedItem == null)
                check = false;

            //TO
            if (CMBB_TO.SelectedItem == null)
                check = false;

            if (check)
            {
                
                AttempConversion();
            }
            else
            {
                MessageWindow messageWindow = new MessageWindow
                {
                    Message = "You Do Not Have Enough Information Entered \nOr You Have An Invalid Custom Extension Identifier\n(Must Begin With '.')"
                };
                messageWindow.Show();
            }
        }
        private void Audio_File_Selection_State_Changed(object sender, PropertyChangedEventArgs e)
        {
            TreeViewModel item = sender as TreeViewModel;

            if (item == null)
                throw new NullReferenceException("Sender Was Not A Tree View Model, was Type: " + sender.GetType().ToString());

            string path = item.Tag as string;

            if (path == null)
                throw new NullReferenceException("Sender Did Not Have A String Type Tag");

            if (item.IsChecked.HasValue)
                if((bool)item.IsChecked)
                    SelectedFiles.Add(path);
            else
                SelectedFiles.Remove(path);

            LB_CHECK.Items.Refresh();
               
        }

        #endregion

        #region ProcessingFunctions

        private void PopulateItemCollectionFromIEnurmable<T>(ItemCollection items, IEnumerable<T> list)
        {
            foreach (T item in list)
                items.Add(item);
        }

        private void GenerateTreeView(TreeView treeView, string path)
        {
            //Iterates all primary sub-directories of given path
            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                //Generate Tree View Node For Each Directory
                TreeViewModel primaryDirectory = new TreeViewModel();
                primaryDirectory.Name = folder.Substring(folder.LastIndexOf('\\') + 1);
                primaryDirectory.Tag = folder;

                //Generate the folders Sub-Directory Nodes
                FillTreeView(primaryDirectory, folder);

                //Initialize Node
                primaryDirectory.Initialize();

                //Add the node to the tree
                treeView.Items.Add(primaryDirectory);
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

                item_file.PropertyChanged += Audio_File_Selection_State_Changed;

                treeView.Items.Add(item_file);

                item_file.Initialize();
            }
        }

        private void FillTreeView(TreeViewModel parentItem, string path)
        {
            foreach (string str in Directory.EnumerateDirectories(path))
            {
                //Generate Tree View Node For Each Directory
                TreeViewModel item = new TreeViewModel();
                item.Name = str.Substring(str.LastIndexOf('\\') + 1);
                item.Tag = str;

                //Add node to parent's descendants
                parentItem.Children.Add(item);

                //Recurse to find additional sub-directories
                FillTreeView(item, str);

                //Catalog All Files As Check Boxes
                foreach (string file in Directory.EnumerateFiles(str))
                {
                    FileInfo fi = new FileInfo(file);

                    //Check to ensure the file type is an acceptable one
                    if (ExtensionUniformer.UnifyExtension(fi.Extension) == null)
                        continue;

                    TreeViewModel item_file = new TreeViewModel();

                    item_file.Name = fi.Name;
                    item_file.Tag = file;

                    item_file.PropertyChanged += Audio_File_Selection_State_Changed;
                    
                    item.Children.Add(item_file);
                }

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

                item_file.PropertyChanged += Audio_File_Selection_State_Changed;

                parentItem.Children.Add(item_file);

                item_file.Initialize();
            }

        }

        private void AttempConversion()
        {
            //tmep code that converts a single file
            ////Grab Target File Path - Eloquent C-Style Casting *caughs*
            //string fileToConvert = ((String)((TreeViewItem)TV_FROM.SelectedItem).Tag);
            //
            //string[] temp = new string[1];
            //temp[0] = fileToConvert;

            //Deep Copy Selected Items So User Can Manipulate List Still
            List<string> targetedFiles = new List<string>(SelectedFiles);

            //Start Up The Progress Reporting Dialog
            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.Maximum = targetedFiles.Count;
            progressDialog.Show();

            //Generate string for destination file type
            string dest = (string)CMBB_TO.SelectedValue;

            //Start A Thread On Conversions
            Thread thread = new Thread(() => Conversion.RunConversions(targetedFiles, dest, progressDialog.UpdateFields, deleteOriginals));

            thread.Start();
            //Thread thread = new Thread(() => Conversion.RunConversions(temp, (string)CMBB_TO.SelectedValue, progressDialog.Progress));
            //
            //thread.Start();
        }


        #endregion

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            deleteOriginals = (bool)RB_DELETE.IsChecked;
        }
    }
}
