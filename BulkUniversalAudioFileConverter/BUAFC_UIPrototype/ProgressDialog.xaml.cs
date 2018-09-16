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

namespace BUAFC_UI
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window
    {

        Thread thread;
        bool threadAborter = false;
        public ProgressDialog()
        {
            InitializeComponent();
            thread = new Thread(PROTOTHREADENTRY);
            thread.Start();
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

        private void PROTOTHREADENTRY()
        {

            for (int i = 0; i < 100; ++i)
            {
                if (threadAborter)
                    break;

                Dispatcher.Invoke(new Action(() => { PRGBR_PROGRESS.Value++; }), null);
                Dispatcher.Invoke(new Action(() => { TXTB_FILE.Content = "Currenly Processing Record " + i + " of 100"; }), null);

                Thread.Sleep(500);
            }

            Thread.Sleep(1000);

            Dispatcher.Invoke(new Action(() => { BUT_OKAY.IsEnabled = true; }), null);
            Dispatcher.Invoke(new Action(() => { BUT_CANCEL.IsEnabled = false; }), null);

            return;
        }
    }
}
