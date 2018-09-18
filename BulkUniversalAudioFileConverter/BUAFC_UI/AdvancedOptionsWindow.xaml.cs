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
using BUAFC_Library;

namespace BUAFC_UI
{
    /// <summary>
    /// Interaction logic for AdvancedOptionsWindow.xaml
    /// </summary>
    public partial class AdvancedOptionsWindow : Window
    {
        public AdvancedOptionsWindow()
        {
            InitializeComponent();
        }

        private void IUD_BITRATE_CHANGED(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Conversion.BitRate = (int)IUD_BITRATE.Value;
        }

        private void IUD_SAMPLE_CHANGED(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Conversion.OGG_SAMPLESIZE1 = (int)IUD_SAMPLE.Value;
        }
    }
}
