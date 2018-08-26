﻿using System;
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

namespace BUAFC_UIPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //Input Supported File Types Into Drop Downs
            CMBB_FROM.Items.Add("ALL...");

            PopulateItemCollectionFromIEnurmable(CMBB_FROM.Items, PROTO_Lists.Extensions);
            PopulateItemCollectionFromIEnurmable(CMBB_AS.Items, PROTO_Lists.Extensions);
            PopulateItemCollectionFromIEnurmable(CMBB_TO.Items, PROTO_Lists.Extensions);

            //Populate Tree Views
            GenerateTreeView(TV_FROM, @"F:\Music");
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
            else if (RB_CUSTOM.IsChecked == true && (CMBB_AS.SelectedItem == null || (TXTBX_RECKOGNIZE.Text[0] != '.')))
                check = false;

            //TO
            if (CMBB_TO.SelectedItem == null)
                check = false;

            if (check)
            {
                ProgressDialog progressDialog = new ProgressDialog();
                progressDialog.Show();
            }
            else
            {
                MessageWindow messageWindow = new MessageWindow();
                messageWindow.Message = "You Do Not Have Enough Information Entered \nOr You Have An Invalid Custom Extension Identifier\n(Must Begin With '.')";
                messageWindow.Show();
            }
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
            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = folder.Substring(folder.LastIndexOf('\\') + 1);
                item.Tag = folder;
                item.FontWeight = FontWeights.Normal;

                FillTreeView(item, folder);
                treeView.Items.Add(item);

                
            }
        }

        private void FillTreeView(TreeViewItem parentItem, string path)
        {
            foreach (string str in Directory.EnumerateDirectories(path))
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = str.Substring(str.LastIndexOf('\\') + 1);
                item.Tag = str;
                item.FontWeight = FontWeights.Normal;
                parentItem.Items.Add(item);
                FillTreeView(item, str);


                foreach (string file in Directory.EnumerateFiles(str))
                {
                    TreeViewItem item_file = new TreeViewItem();
                    FileInfo fi = new FileInfo(file);

                    item_file.Header = fi.Name;
                    item_file.Tag = file;
                    item_file.FontWeight = FontWeights.Normal;
                    
                    item.Items.Add(item_file);
                }

            }

        }

        #endregion
    }
}
