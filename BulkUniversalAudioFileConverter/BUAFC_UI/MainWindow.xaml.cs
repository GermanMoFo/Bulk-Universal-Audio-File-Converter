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

        List<string> TruncatedFiles = new List<string>();
        private int numberDirectoriesToTruncate = 0;

        bool deleteOriginals = false;

        string PrimaryDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

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
            Conversion.Initialize();

            //Populate Tree Views
            RefreshTreeView();
            

            //Link Checking ListBox
            LB_CHECK.ItemsSource = TruncatedFiles;

            //Initialize Primary Directory Text
            TXTB_PRIMARYDIRECTORY.Text = PrimaryDirectoryPath;
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

            RefreshTruncatedList();
            LB_CHECK.Items.Refresh();
               
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            deleteOriginals = (bool)RB_DELETE.IsChecked;
        }

        private void BUT_ADV_Click(object sender, RoutedEventArgs e)
        {
            AdvancedOptionsWindow window = new AdvancedOptionsWindow();
            window.Show();

        }

        private void BUT_DISPLAYMORE_Click(object sender, RoutedEventArgs e)
        {
            --numberDirectoriesToTruncate;

            if (numberDirectoriesToTruncate == 0)
                BUT_DISPLAYMORE.IsEnabled = false;

            RefreshTruncatedList();
            LB_CHECK.Items.Refresh();
        }

        private void BUT_DISPLAYLESS_Click(object sender, RoutedEventArgs e)
        {
            ++numberDirectoriesToTruncate;

            BUT_DISPLAYMORE.IsEnabled = true;

            RefreshTruncatedList();
            LB_CHECK.Items.Refresh();
        }

        private void BUT_BROWSEPRIMARYDIRECTORY_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog hello = new FolderBrowserDialog();

            if (hello.ShowDialog() == true)
            {
                PrimaryDirectoryPath = hello.SelectedPath;
                SelectedFiles.Clear();
                RefreshTruncatedList();
                RefreshTreeView();
            }

        }

        private void BUT_REFRESH_Click(object sender, RoutedEventArgs e)
        {
            RefreshTreeView();
        }

        #endregion

        #region ProcessingFunctions

        private void RefreshTruncatedList()
        {
            TruncatedFiles.Clear();

            foreach (var path in SelectedFiles)
                TruncatedFiles.Add(UI_LIB.TruncatePathToDirectory(path, numberDirectoriesToTruncate));

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
            RefreshTruncatedList();
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

                RefreshTruncatedList();
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



        private void CMBB_TO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TXTB_PRIMARYDIRECTORY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox)
            {
                string temp;
                var text = sender as TextBox;

                try
                {
                    temp = System.IO.Path.GetFullPath(text.Text);
                    PrimaryDirectoryPath = temp;
                    RefreshTreeView();
                }
                catch (ArgumentException ae)
                {

                }
            }
        }
    }
}
