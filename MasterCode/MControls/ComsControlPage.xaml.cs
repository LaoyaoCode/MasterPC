using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterCode.MControls
{
    /// <summary>
    /// ComsControlPage.xaml 的交互逻辑
    /// </summary>
    public partial class ComsControlPage : UserControl
    {
        //radio button 就算使用 Parity1RB.IsChecked = true; 
        //软件设置其是否被check
        //也会触发点击事件


        public ComsControlPage()
        {
            InitializeComponent();
        }

        //串口开启
        private void ComOpenOrCloseTB_Checked(object sender, RoutedEventArgs e)
        {
            ComsSelectSP.IsEnabled = false;
            StopBitsSP.IsEnabled = false;
            ParitySP.IsEnabled = false;
            HandshakeSP.IsEnabled = false;
            BandRateSP.IsEnabled = false;
        }

        //串口关闭
        private void ComOpenOrCloseTB_Unchecked(object sender, RoutedEventArgs e)
        {
            ComsSelectSP.IsEnabled = true;
            StopBitsSP.IsEnabled = true;
            ParitySP.IsEnabled = true;
            HandshakeSP.IsEnabled = true;
            BandRateSP.IsEnabled = true;
        }

        private void StopRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
