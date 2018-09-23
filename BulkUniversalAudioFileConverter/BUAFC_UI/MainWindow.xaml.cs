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
using Avalon.Windows.Dialogs;

namespace BUAFC_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> SelectedFiles = new List<string>();
        List<ListBoxItem> TruncatedFiles = new List<ListBoxItem>();

        private int numberDirectoriesToTruncate = 1;

        bool deleteOriginals = false;

        string PrimaryDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

        public MainWindow()
        {
            InitializeComponent();

            //Input Supported File Types Into Drop Downs
            PopulateItemCollectionFromIEnurmable(CMBB_TO.Items,   Conversion.SupportedExtensions);

            //Input FilemodeOptions 
            PopulateItemCollectionFromIEnurmable(CB_DEST.Items, UI_Lists.FilemodeOptions);

            //Generate Extension Unifiers To Accomodate Weird Extensions
            UI_LIB.LoadAlternateExtensions();

            //Generate Conversion Method Library
            Conversion.Initialize();

            //Link Checking ListBox
            LB_CHECK.Items.Clear();
            LB_CHECK.ItemsSource = TruncatedFiles;

            //Populate Tree Views
            RefreshTreeView();

            //Initialize Primary Directory Text
            TXTB_PRIMARYDIRECTORY.Text = PrimaryDirectoryPath;

            //Initialize CheckedComboBox
            CCB_FROM.ItemsSource = UI_Lists.Extensions;
        }



        #region EventHandlers

        private void CMBB_TO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshCheckPreview();
        }
        
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if(radioButton != null)
            {
                if (radioButton.Name == "RB_ALL")
                {
                    if (CCB_FROM != null)
                    {
                        CCB_FROM.SelectedItems.Clear();
                    }

                }
            }
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {

            //VERIFY ENOUGH OPTIONS ARE SET
            bool check = true;
            string errorMsg = "You Do Not Have Enough Information Entered.";

            //FROM
            if (RB_STRICT.IsChecked == true && CCB_FROM.SelectedItem == null)
            {
                check = false;
                errorMsg += "\nPlease ensure you have selected either ''All...'' Or ''Filter'',\nif filter is selected please enter a selection";
            }

            //TO
            if (CMBB_TO.SelectedItem == null)
            {
                check = false;
                errorMsg += "\nPlease ensure you have selected a target file type in ''To...''.";
            }

            if (check)
                AttempConversion();
            else
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.Message = errorMsg;
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

            RefreshCheckPreview();
            LB_CHECK.Items.Refresh();
               
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            deleteOriginals = (bool)RB_DELETE.IsChecked;
        }

        private void BUT_DISPLAYLESS_Click(object sender, RoutedEventArgs e)
        {
            ++numberDirectoriesToTruncate;

            BUT_DISPLAYMORE.IsEnabled = true;

            var holdforcomparison = new List<ListBoxItem>(TruncatedFiles);

            RefreshCheckPreview();

            if (CheckListsForSequenceEquality(holdforcomparison, TruncatedFiles))
            {
                numberDirectoriesToTruncate--;
                BUT_DISPLAYLESS.IsEnabled = false;
            }

            LB_CHECK.Items.Refresh();

            bool CheckListsForSequenceEquality(List<ListBoxItem> A, List<ListBoxItem> B)
            {
                if (A.Count != B.Count)
                    return false;

                for (int i = 0; i < A.Count; ++i)
                    if ((string)A[i].Content != (string)B[i].Content)
                        return false;

                return true;
            }
        }

        private void BUT_DISPLAYMORE_Click(object sender, RoutedEventArgs e)
        {
            --numberDirectoriesToTruncate;

            BUT_DISPLAYLESS.IsEnabled = true;

            if (numberDirectoriesToTruncate == 0)
                numberDirectoriesToTruncate = 1;

            if (numberDirectoriesToTruncate == 1)
                BUT_DISPLAYMORE.IsEnabled = false;

            RefreshCheckPreview();
        }

        private void BUT_BROWSEPRIMARYDIRECTORY_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog hello = new FolderBrowserDialog();

            if (hello.ShowDialog() == true)
            {
                PrimaryDirectoryPath = hello.SelectedPath;
                SelectedFiles.Clear();
                RefreshCheckPreview();
                RefreshTreeView();
            }

        }

        private void BUT_REFRESH_Click(object sender, RoutedEventArgs e)
        {
            RefreshTreeView();
        }

        private void CB_DEST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //"In-Place", "Directory Dump", "Smart Dump"
            //Conversion
            switch ((String)CB_DEST.SelectedItem)
            {
                case "In-Place":
                    Conversion.PathMode = Conversion.PathModeType.InPlace;
                    TXT_DEST.IsEnabled = false;
                    BUT_BROWSEDEST_Copy.IsEnabled = false;
                    break;
                case "Directory Dump":
                    Conversion.PathMode = Conversion.PathModeType.DirectoryDump;
                    TXT_DEST.IsEnabled = true;
                    BUT_BROWSEDEST_Copy.IsEnabled = true;
                    break;
                case "Smart Dump":
                    Conversion.PathMode = Conversion.PathModeType.SmartDump;
                    TXT_DEST.IsEnabled = true;
                    BUT_BROWSEDEST_Copy.IsEnabled = true;
                    break;
                default:
                    throw new ArgumentException();
            }

            RefreshCheckPreview();
        }

        private void TXTB_PRIMARYDIRECTORY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox)
            {
                string temp;
                var text = sender as TextBox;

                try
                {
                    //See If Directory Is Formated Validly
                    temp = System.IO.Path.GetFullPath(text.Text);
                    //Check To See If Directory Exists
                    if (!Directory.Exists(temp))
                        throw new ArgumentException();


                    text.Text = temp;
                    PrimaryDirectoryPath = temp;
                    RefreshTreeView();
                }
                catch 
                {
                    MessageBoxResult result = MessageBox.Show("Entered Path: " + text.Text + "\nWas Invalid, Please Enter A Valid, Existing Path", "Confirm", MessageBoxButton.OK);
                }
            }
        }

        private void TXTB_DEST_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox)
            {
                string temp;
                var text = sender as TextBox;

                try
                {
                    //See If Directory Is Formated Validly
                    temp = System.IO.Path.GetFullPath(text.Text);

                    text.Text = temp;
                    Conversion.UserSpecifiedDirectory = temp;
                    RefreshCheckPreview();

                }
                catch
                {
                    MessageBoxResult result = MessageBox.Show("Entered Path: " + text.Text + "\nWas Invalid, Please Enter A Correct Path", "Confirm", MessageBoxButton.OK);
                }
            }
        }

        private void CCB_FROM_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            if (CCB_FROM.SelectedItems.Count != 0)
            {
                RB_ALL.IsChecked = false;
                RB_STRICT.IsChecked = true;
            }
        }

        private void TXTB_GENERAL_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = e.OriginalSource as TextBox;

            if (box != null)
            {
                box.SelectAll();
                box.Focus();
            }
        }

        private void BUT_BROWSEDEST_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog hello = new FolderBrowserDialog();

            if (hello.ShowDialog() == true)
            {
                Conversion.UserSpecifiedDirectory = hello.SelectedPath;
                TXT_DEST.Text = hello.SelectedPath;
                RefreshCheckPreview();
            }
        }

        private void IUD_BITRATE_CHANGED(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Conversion.BitRate = (int)IUD_BITRATE.Value;
        }

        private void IUD_SAMPLE_CHANGED(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Conversion.OGG_SAMPLESIZE1 = (int)IUD_SAMPLE.Value;
        }

        #endregion

        #region ProcessingFunctions

        private void RefreshCheckPreview()
        {
            TruncatedFiles.Clear();

            string LCD = "";
            if (Conversion.PathMode == Conversion.PathModeType.SmartDump)
                LCD = BUAFC_Library.Utitlities.FindCommonPath("\\", SelectedFiles);

            for (int i = 0; i < SelectedFiles.Count; ++i)
            {
                ListBoxItem temp = new ListBoxItem();
                temp.Content = UI_LIB.TruncatePathToDirectory(SelectedFiles[i], numberDirectoriesToTruncate);
                temp.ToolTip = SelectedFiles[i] + '\n' + "Will be converted to:" + '\n';
                TruncatedFiles.Add(temp);

                switch (Conversion.PathMode)
                {
                    case Conversion.PathModeType.DirectoryDump:
                        temp.ToolTip += Conversion.UserSpecifiedDirectory + "\\" + System.IO.Path.GetFileName(SelectedFiles[i]).Remove(System.IO.Path.GetFileName(SelectedFiles[i]).LastIndexOf('.')) + CMBB_TO.SelectedValue;
                        break;
                    case Conversion.PathModeType.InPlace:
                        temp.ToolTip += SelectedFiles[i].Remove(SelectedFiles[i].LastIndexOf('.')) + CMBB_TO.SelectedValue;
                        break;
                    case Conversion.PathModeType.SmartDump:
                        FileInfo fi = new FileInfo(Conversion.UserSpecifiedDirectory + SelectedFiles[i].Remove(0, LCD.Length));
                        fi.Directory.Create();
                        if (fi.FullName.LastIndexOf('.') != "".LastIndexOf('.'))
                            temp.ToolTip += fi.FullName.Remove(fi.FullName.LastIndexOf('.')) + CMBB_TO.SelectedValue;
                        break;
                }

            }

            LB_CHECK.Items.Refresh();

        }

        /// <summary>
        /// Intensive For Large Libraries, only used when neccasarry
        /// Or as intended by user.
        /// </summary>
        public void RefreshTreeView()
        {
            //Destroy Old ItemSource
            TV_FROM.ItemsSource = null;

            //Cache All Selected Files So We Can Re-Determine What Was Selected
            var oldSelectedList = SelectedFiles;

            //Clear Selected Files
            SelectedFiles = new List<string>();
            RefreshCheckPreview();
            LB_CHECK.Items.Refresh();

            //Loading Notification Show
            UserControl loadAnimation = LD_FROM as UserControl;
            loadAnimation.Visibility = Visibility.Visible;

            //Generate New ItemSource 
            Thread thread = new Thread(() =>
                FolderTreeViewManager.Initialize(
                    TV_FROM,
                    PrimaryDirectoryPath,
                    Audio_File_Selection_State_Changed, new Action(() => { loadAnimation.Visibility = Visibility.Hidden; TransferSelection(oldSelectedList); })
                )
            );
            thread.Start();
            
            //Stored For Action Simplification With Callback
            void TransferSelection(List<string> old)
            {
                //Itterate New ItemSource and "Select" files whose path used to be selected
                foreach (var pair in FolderTreeViewManager.NodeList)
                {
                    if (oldSelectedList.Contains(pair.a))
                        pair.b.IsChecked = true;
                }

                RefreshCheckPreview();
                TV_FROM.Items.Refresh();
            }

        }

        private void PopulateItemCollectionFromIEnurmable<T>(ItemCollection items, IEnumerable<T> list)
        {
            foreach (T item in list)
                items.Add(item);
        }

        private void AttempConversion()
        {
            //Deep Copy Selected Items So User Can Manipulate List Still, And Filter Based On User-Desired Extensions
            List<string> targetedFiles = new List<string>();

            foreach (var file in SelectedFiles)
            {
                if (null != ExtensionUniformer.UnifyExtension(System.IO.Path.GetExtension(file)))
                {
                    if ((bool)RB_STRICT.IsChecked)
                        foreach (string extension in CCB_FROM.SelectedItems)
                            if (extension == System.IO.Path.GetExtension(file))
                                targetedFiles.Add(file);
                }
                else if ((bool)RB_ALL.IsChecked)
                    targetedFiles.Add(file);
            }

            //Start Up The Progress Reporting Dialog
            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.Maximum = targetedFiles.Count;
            progressDialog.Show();

            //Generate string for destination file type
            string dest = (string)CMBB_TO.SelectedValue;

            //Start A Thread On Conversions
            Thread thread = new Thread(() => Conversion.RunConversions(targetedFiles, dest, progressDialog.UpdateFields, deleteOriginals));
            thread.Start();
        }



        #endregion

        
    }
}
