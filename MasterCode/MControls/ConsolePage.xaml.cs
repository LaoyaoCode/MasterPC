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
    /// ConsolePage.xaml 的交互逻辑
    /// </summary>
    public partial class ConsolePage : UserControl
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        public static ConsolePage UnityIns = null;

        /// <summary>
        /// 控制台增加了一个信息代理事件
        /// </summary>
        public delegate void ConsoleAddAMessageDel();

        public event ConsoleAddAMessageDel ConsoleAddAMessage;

        public ConsolePage()
        {
            InitializeComponent();

            UnityIns = this;
        }

        public void AddMessage(AConsoleMessage.MessageKindEnum kind , String message)
        {
            AConsoleMessage consoleMessage = new AConsoleMessage(kind, message);
            ConsoleMessageSP.Children.Add(consoleMessage);

            if(ConsoleAddAMessage != null)
            {
                ConsoleAddAMessage.Invoke();
            }
        }
    }
}
