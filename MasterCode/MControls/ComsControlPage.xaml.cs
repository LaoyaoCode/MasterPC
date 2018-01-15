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
using MasterCode.Code;

namespace MasterCode.MControls
{
    /// <summary>
    /// ComsControlPage.xaml 的交互逻辑
    /// </summary>
    public partial class ComsControlPage : UserControl
    {
        private List<ImageTextButton> TotalPorts = new List<ImageTextButton>();
        private ImageTextButton NowSelectComs = null;
        private System.Windows.Threading.DispatcherTimer InsideSecondTimer = new System.Windows.Threading.DispatcherTimer();//初始化时钟

        //radio button 就算使用 Parity1RB.IsChecked = true; 
        //软件设置其是否被check
        //也会触发点击事件
        public ComsControlPage()
        {
            InitializeComponent();

            InsideSecondTimer.Interval = TimeSpan.FromSeconds(1);//每一秒触发一次
            InsideSecondTimer.IsEnabled = true;//开启定时器
            InsideSecondTimer.Tick += InsideSecondTimer_Tick; ;//添加时间代理
            InsideSecondTimer.Start();
        }

        private void InsideSecondTimer_Tick(object sender, EventArgs e)
        {
            List<string> names = ComNamesDispose.GetComNames();

            //上次选择的串口已经消失 , 则去除
            //不需要担心串口的开关问题，其已经由事件解决
            if (NowSelectComs != null && !names.Contains(NowSelectComs.MButtonText))
            {
                NowSelectComs.DisActive();
                ComsSelectSP.Children.Remove(NowSelectComs);
                TotalPorts.Remove(NowSelectComs);
                NowSelectComs = null;
            }

            //去除已经不存在的串口
            List<int> removeIndex = new List<int>();
            for(int counter = 0; counter < TotalPorts.Count; counter++)
            {
                if (!names.Contains(TotalPorts[counter].MButtonText))
                {
                    removeIndex.Add(counter);
                }
            }
       
            foreach (int index in removeIndex)
            {
                TotalPorts.RemoveAt(index);
                ComsSelectSP.Children.RemoveAt(index);
            }
           

            List<string> nowDisplayComs = new List<string>();
            foreach (var port in TotalPorts)
            {
                nowDisplayComs.Add(port.MButtonText);
            }

            foreach (var name in names)
            {
                //如果出现了新的串口,将其添加
                if(!nowDisplayComs.Contains(name))
                {
                    ImageTextButton comButton = new ImageTextButton();
                    comButton.Style = (Style)this.Resources["MComSelectTButtonStyle"];
                    comButton.SetIDAndClick(ComsSelectButtonClick, name);
                    comButton.MButtonText = name;

                    ComsSelectSP.Children.Add(comButton);
                    TotalPorts.Add(comButton);
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ComsSelectButtonClick(ImageTextButton sender , String id)
        {

        }

        //串口开启
        private void ComOpenOrCloseTB_Checked(object sender, RoutedEventArgs e)
        {
            ComsSelectSP.IsEnabled = false;
            StopBitsSP.IsEnabled = false;
            ParitySP.IsEnabled = false;
            HandshakeSP.IsEnabled = false;
            BandRateSP.IsEnabled = false;
            ComOpenOrCloseTB.ToolTip = "关闭串口";
        }

        //串口关闭
        private void ComOpenOrCloseTB_Unchecked(object sender, RoutedEventArgs e)
        {
            ComsSelectSP.IsEnabled = true;
            StopBitsSP.IsEnabled = true;
            ParitySP.IsEnabled = true;
            HandshakeSP.IsEnabled = true;
            BandRateSP.IsEnabled = true;
            ComOpenOrCloseTB.ToolTip = "打开串口";
        }

        private void StopRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
