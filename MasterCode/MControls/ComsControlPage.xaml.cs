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
using System.IO.Ports;
using MasterCode.Code.Tools;

namespace MasterCode.MControls
{
    /// <summary>
    /// ComsControlPage.xaml 的交互逻辑
    /// </summary>
    public partial class ComsControlPage : UserControl
    {
        /// <summary>
        /// 选择的串口参数
        /// </summary>
        private ComControler.PortParaSetStruct SelectPara = new ComControler.PortParaSetStruct();
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

            ComOpenOrCloseTB.IsEnabled = false;
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

                //如果在串口消失即拔出串口的时候，串口已经打开了，则自动关闭
                if(ComControler.UnityIns.IsPortOpen())
                {
                    ComControler.UnityIns.PortClosed();
                    ComOpenOrCloseTB.IsChecked = false;
                }
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

            if (NowSelectComs == null)
            {
                ComOpenOrCloseTB.IsEnabled = false;
            }
            else
            {
                ComOpenOrCloseTB.IsEnabled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string stopBitsTag = StopBitsEnumToTag(UserPerferControler.UnityIns.GetPortPara().ChooseStopBits);
            foreach (var item in StopBitGrid.Children)
            {
                if ((string)((RadioButton)item).Tag == stopBitsTag)
                {
                    ((RadioButton)item).IsChecked = true;
                    SelectPara.ChooseStopBits = StopBitsTagToEnum((string)((RadioButton)item).Tag);
                }
            }

            string parityTag = ParityEnumToTag(UserPerferControler.UnityIns.GetPortPara().ChooseParity);
            foreach (var item in ParityGrid.Children)
            {
                if(((string)((RadioButton)item).Tag == parityTag))
                {
                    ((RadioButton)item).IsChecked = true;
                    SelectPara.ChooseParity = ParityTagToEnum((string)((RadioButton)item).Tag);
                }
            }

            string handShakeTag = HandShakeEnumToTag(UserPerferControler.UnityIns.GetPortPara().ChooseHandShake);
            foreach (var item in HandShakeGrid.Children)
            {
                if (((string)((RadioButton)item).Tag == handShakeTag))
                {
                    ((RadioButton)item).IsChecked = true;
                    SelectPara.ChooseHandShake = HandShakeTagToEnum((string)((RadioButton)item).Tag);
                }
            }

            string baudRateTag = UserPerferControler.UnityIns.GetPortPara().BaudRate.ToString();
            foreach (var item in BaudRateGrid.Children)
            {
                if (((string)((RadioButton)item).Tag == baudRateTag))
                {
                    ((RadioButton)item).IsChecked = true;
                    SelectPara.BaudRate = int.Parse((string)((RadioButton)item).Tag);
                }
            }
        }

        private void ComsSelectButtonClick(ImageTextButton sender , String id)
        {
            if(NowSelectComs != null && NowSelectComs !=  sender)
            {
                NowSelectComs.DisActive();
            }

            NowSelectComs = sender;

            //串口名字选择赋值
            SelectPara.ComName = NowSelectComs.MButtonText;
        }


        private void StopRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectPara.ChooseStopBits = StopBitsTagToEnum((string)((RadioButton)sender).Tag);
        }

        private void ParityButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectPara.ChooseParity = ParityTagToEnum((string)((RadioButton)sender).Tag);
        }

        private void HandShakeButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectPara.ChooseHandShake = HandShakeTagToEnum((string)((RadioButton)sender).Tag);
        }

        private void BaudRateButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectPara.BaudRate = int.Parse((string)((RadioButton)sender).Tag);
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

            //保存之前开启串口的时候的参数
            UserPerferControler.UnityIns.SetPortPara(SelectPara);
            ComControler.UnityIns.SetPortPara(SelectPara);

            ComControler.UnityIns.OpenPort();



            //test--------------------
            ComControler.UnityIns.SendCommandToMCU(ComControler.CommandEnum.BeginTrans);
            ComControler.UnityIns.SendCommandEvent += (x) =>
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if(x)
                    {
                        ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "串口数据发送成功");
                    }
                    else
                    {
                        ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Error, "串口数据发送失败");
                    }
                }
                ));
            };
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
            //关闭串口
            ComControler.UnityIns.PortClosed();
        }

        /// <summary>
        /// 将停止位枚举值转化为标签值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private string StopBitsEnumToTag(StopBits enumValue)
        {
           string value;

           switch(enumValue)
           {
                case StopBits.One:
                    value = "1";
                    break;
                case StopBits.OnePointFive:
                    value = "1.5";
                    break;
                case StopBits.Two:
                    value = "2";
                    break;
                default:
                    value = "error";
                    break;
            }

            return value;
        }

        /// <summary>
        /// 将停止位标签转化为枚举值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private StopBits StopBitsTagToEnum(string tag)
        {
            if(tag == "1")
            {
                return StopBits.One;
            }
            else if(tag == "1.5")
            {
                return StopBits.OnePointFive;
            }
            else if(tag == "2")
            {
                return StopBits.Two;
            }
            else
            {
                throw new Exception("Tag Not Exist");
            }
        }

        /// <summary>
        /// 将奇偶校验位枚举值转化为标签值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private string ParityEnumToTag(Parity enumValue)
        {
            string value = "error";

            switch(enumValue)
            {
                case Parity.None:
                    value = "None";
                    break;
                case Parity.Even:
                    value = "Even";
                    break;
                case Parity.Mark:
                    value = "Mark";
                    break;
                case Parity.Odd:
                    value = "Odd";
                    break;
                case Parity.Space:
                    value = "Space";
                    break;
            }

            return value;
        }

        private Parity ParityTagToEnum(string tag)
        {
            if(tag == "None")
            {
                return Parity.None;
            }
            else if(tag == "Even")
            {
                return Parity.Even;
            }
            else if(tag == "Mark")
            {
                return Parity.Mark;
            }
            else if(tag == "Odd")
            {
                return Parity.Odd;
            }
            else if(tag == "Space")
            {
                return Parity.Space;
            }
            else
            {
                throw new Exception("Tag Not Exist");
            }
        }

        /// <summary>
        /// 将握手协议枚举值转化为标签值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private string HandShakeEnumToTag(Handshake enumValue)
        {
            string value = "error";

            switch(enumValue)
            {
                case Handshake.None:
                    value = "None";
                    break;
                case Handshake.RequestToSend:
                    value = "RTS";
                    break;
                case Handshake.RequestToSendXOnXOff:
                    value = "RTSXOXF";
                    break;
                case Handshake.XOnXOff:
                    value = "XOXF";
                    break;
            }

            return value;
        }


        /// <summary>
        /// 将握手协议枚举值转化为标签值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private Handshake HandShakeTagToEnum(string tag)
        {
            if (tag == "None")
            {
                return Handshake.None;
            }
            else if (tag == "RTS")
            {
                return Handshake.RequestToSend;
            }
            else if (tag == "RTSXOXF")
            {
                return Handshake.RequestToSendXOnXOff;
            }
            else if (tag == "XOXF")
            {
                return Handshake.XOnXOff;
            }
            else
            {
                throw new Exception("Tag Not Exist");
            }
        }
    }
}
