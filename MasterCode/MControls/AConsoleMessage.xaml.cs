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
    /// AConsoleMessage.xaml 的交互逻辑
    /// </summary>
    public partial class AConsoleMessage : UserControl
    {
        /// <summary>
        /// 信息的种类
        /// </summary>
        public enum MessageKindEnum
        {
            /// <summary>
            /// 正常信息
            /// </summary>
            Normal,
            /// <summary>
            /// 重要信息
            /// </summary>
            Important,
            /// <summary>
            /// 错误信息
            /// </summary>
            Error
        }



        private MessageKindEnum MessageKind = MessageKindEnum.Normal;
        private String Message = String.Empty;


        /// <summary>
        /// 一条控制台信息
        /// </summary>
        /// <param name="kind">信息种类</param>
        /// <param name="message">信息</param>
        public AConsoleMessage(MessageKindEnum kind , String message)
        {
            InitializeComponent();

            MessageKind = kind;
            Message = message;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            String colorKey = String.Empty;

            switch (MessageKind)
            {
                case MessageKindEnum.Normal:
                    colorKey = "NormalTextColor";
                    break;
                case MessageKindEnum.Important:
                    colorKey = "LeftThumbBrush";
                    break;
                case MessageKindEnum.Error:
                    colorKey = "RightThumbBrush";
                    break;
            }

            MessageIcon.Foreground = (SolidColorBrush)this.FindResource(colorKey);
            MessageTB.Foreground = (SolidColorBrush)this.FindResource(colorKey);
            MessageTB.Text = Message;
        }
    }
}
