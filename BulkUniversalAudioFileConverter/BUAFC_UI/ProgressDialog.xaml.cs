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
        
        bool threadAborter = false;

        public ProgressBar Progress
        {
            get
            {
                return PRGBR_PROGRESS;
            }
        }


        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void BUT_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            threadAborter = true;
            TXTB_STAGE.Content = "Canceling Progress Please Wait...";
        }

        private void BUT_OKAY_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    
}
