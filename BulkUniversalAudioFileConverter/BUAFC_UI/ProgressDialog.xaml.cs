using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Threading;
using BUAFC_Library;

namespace BUAFC_UI
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window
    {
        private bool threadAborter = false;

        public bool ThreadAborter { get => threadAborter; }

        public double Maximum { get => PRGBR_PROGRESS.Maximum; set => PRGBR_PROGRESS.Maximum = value; }

        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void BUT_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            threadAborter = true;
            TXTB_Action.Content = "Canceling Progress Please Wait...";
        }

        private void BUT_OKAY_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        public void UpdateFields(string CurrentDirectory, string CurrentAction, int Progress)
        {
            TXTB_Directory.Content = CurrentDirectory;
            TXTB_Action.Content = CurrentAction;
            TXTB_Prog.Content = Progress.ToString() + " / " + PRGBR_PROGRESS.Maximum.ToString();
            PRGBR_PROGRESS.Value = Progress;

            if(Progress == PRGBR_PROGRESS.Maximum)
            {
                //Conversion Is Done, Disable Cancel, Enable Okay
                BUT_CANCEL.IsEnabled = false;
                BUT_OKAY.IsEnabled = true;
            }
        }
    }

    
}
