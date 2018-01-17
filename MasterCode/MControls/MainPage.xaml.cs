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
using MasterCode.Code.Tools;
using MasterCode.Code;
using System.IO;
using Dialog = System.Windows.Forms;

namespace MasterCode.MControls
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// 进度条矩形的初始长度
        /// </summary>
        private const double OriginalPercentLineWidth = 580.0;
        private System.Windows.Threading.DispatcherTimer InsideSecondTimer = new System.Windows.Threading.DispatcherTimer();//初始化时钟

        //一个小时所需要的秒数
        private const int PeroidOneHourSeconds = 60 * 60;
        private int PeroidSeconds = 0;
        private int NowPeroidSeconds = 0;

        public MainPage()
        {
            InitializeComponent();

            NowPeroidSeconds = 0;

            //注册进度条事件
            ComControler.DatasCollectProgressEvent += ComControler_DatasCollectProgressEvent;
        }

        /// <summary>
        /// 当主程序初始化完成，请调用此函数
        /// </summary>
        public void WhenInitFinished()
        {
            //计算周期数据，设定时间
            PeroidSeconds = PeroidOneHourSeconds * UserPerferControler.UnityIns.GetPeroidHours();
            ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Important, "周期 : " + UserPerferControler.UnityIns.GetPeroidHours().ToString() + " 小时 -- " + PeroidSeconds.ToString() + "秒");

            InsideSecondTimer.Interval = TimeSpan.FromSeconds(1);//每一秒触发一次
            InsideSecondTimer.IsEnabled = true;//开启定时器
            InsideSecondTimer.Tick += InsideSecondTimer_Tick;
            InsideSecondTimer.Start();
        }

        private void InsideSecondTimer_Tick(object sender, EventArgs e)
        {
            //周期进度显示
            NowPeroidSeconds++;
            PeriodPercentLine.Width = ((double)NowPeroidSeconds) / PeroidSeconds * OriginalPercentLineWidth;

            //达到了指定事件，则开始采集
            if(NowPeroidSeconds == PeroidSeconds)
            {
                //如果串口打开了，则开始采集
                if(ComControler.UnityIns.IsPortOpen())
                {
                    //发送开始采集，命令
                    ComControler.UnityIns.SendCommandToMCU(ComControler.CommandEnum.BeginTrans);
                    NowPeroidSeconds = 0;
                }
                //串口还未打开，则提示
                else
                {
                    ConsolePage.UnityIns.AddMessage(AConsoleMessage.MessageKindEnum.Error, "串口未打开，采集数据取消");
                    NowPeroidSeconds = 0;
                }
            }
        }

        private void ComControler_DatasCollectProgressEvent(float percent)
        {
            DatasGetPercentLine.Width = OriginalPercentLineWidth * percent;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserPerferControler.UnityIns.GetExcelPath() == "NULL")
            {
                ExcelPathTBlock.Text = "Excel Datas Dir : " + PathStaicCollection.DefaultExcelDir;
            }
            else
            {
                ExcelPathTBlock.Text = "Excel Datas Dir : " + UserPerferControler.UnityIns.GetExcelPath();
            }

            PeroidSetSlider.Value = UserPerferControler.UnityIns.GetPeroidHours();
        }

        public void CLose()
        {
            //保存设定
            UserPerferControler.UnityIns.SetPeroidHours((int)PeroidSetSlider.Value);
        }

        private void ChangeExcelPathButton_Click()
        {
            Dialog.FolderBrowserDialog m_Dialog = new Dialog.FolderBrowserDialog();
            m_Dialog.Description = "选择产生的Excel文件保存位置";
            Dialog.DialogResult result = m_Dialog.ShowDialog();

            //如果什么都没有选择，则直接返回
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            UserPerferControler.UnityIns.SetExcelPath(m_Dialog.SelectedPath.Trim());
            ExcelPathTBlock.Text = "Excel Datas Dir : " + UserPerferControler.UnityIns.GetExcelPath();
        }

        private void OpenExcelPathButton_Click()
        {
            String path;
            if (UserPerferControler.UnityIns.GetExcelPath() == "NULL")
            {
                path = PathStaicCollection.DefaultExcelDir;
            }
            else
            {
                path = UserPerferControler.UnityIns.GetExcelPath();
            }

            System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }
}
