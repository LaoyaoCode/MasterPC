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
    /// ImageTextButton.xaml 的交互逻辑
    /// </summary>
    public partial class ImageTextButton : UserControl
    {
        public delegate void ButtonClickEvent(ImageTextButton sender , String id);

        /// <summary>
        /// 按钮文本
        /// </summary>
        public string MButtonText
        {
            get { return (string)GetValue(MButtonTextProperty); }
            set { SetValue(MButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MButtonTextProperty =
            DependencyProperty.Register("MButtonText", typeof(string), typeof(ImageTextButton), new PropertyMetadata(null));


        /// <summary>
        /// 正常情况下的背景
        /// </summary>
        public SolidColorBrush NormalBackBrush
        {
            get { return (SolidColorBrush)GetValue(NormalBackBrushProperty); }
            set { SetValue(NormalBackBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalBackBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalBackBrushProperty =
            DependencyProperty.Register("NormalBackBrush", typeof(SolidColorBrush), typeof(ImageTextButton), new PropertyMetadata(null));



        /// <summary>
        /// 鼠标进入背景
        /// </summary>
        public SolidColorBrush MouseEnterBackBrush
        {
            get { return (SolidColorBrush)GetValue(MouseEnterBackBrushProperty); }
            set { SetValue(MouseEnterBackBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseEnterBackBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseEnterBackBrushProperty =
            DependencyProperty.Register("MouseEnterBackBrush", typeof(SolidColorBrush), typeof(ImageTextButton), new PropertyMetadata(null));



        /// <summary>
        /// 鼠标按下背景
        /// </summary>
        public SolidColorBrush MouseDownBackBrush
        {
            get { return (SolidColorBrush)GetValue(MouseDownBackBrushProperty); }
            set { SetValue(MouseDownBackBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseDownBackBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseDownBackBrushProperty =
            DependencyProperty.Register("MouseDownBackBrush", typeof(SolidColorBrush), typeof(ImageTextButton), new PropertyMetadata(null));



        /// <summary>
        /// 显示的图标图片源
        /// </summary>
        public ImageSource DisplayIconSource
        {
            get { return (ImageSource)GetValue(DisplayIconSourceProperty); }
            set { SetValue(DisplayIconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayIconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayIconSourceProperty =
            DependencyProperty.Register("DisplayIconSource", typeof(ImageSource), typeof(ImageTextButton), new PropertyMetadata(null));



        /// <summary>
        /// 点击活跃显示矩形颜色
        /// </summary>
        public SolidColorBrush ActiveRectBrush
        {
            get { return (SolidColorBrush)GetValue(ActiveRectBrushProperty); }
            set { SetValue(ActiveRectBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveRectBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveRectBrushProperty =
            DependencyProperty.Register("ActiveRectBrush", typeof(SolidColorBrush), typeof(ImageTextButton), new PropertyMetadata(null));


        private bool IsActive = false;
        private String ButtonID = "NULL";
        private event ButtonClickEvent ButtonClick;
        private int MessageCount = 0 ;

        public ImageTextButton()
        {
            InitializeComponent();

            //隐藏活跃标志
            ActiveTagRect.Visibility = Visibility.Collapsed;
            //设置背景
            RootGrid.Background = NormalBackBrush;

        }

        /// <summary>
        /// 设定点击事件代理和按钮ID
        /// </summary>
        /// <param name="click">点击事件</param>
        /// <param name="id">ID</param>
        public void SetIDAndClick(ButtonClickEvent click , String id)
        {
            ButtonClick += click;
            ButtonID = id;
        }

        private void RootGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            RootGrid.Background = MouseEnterBackBrush;
        }

        private void RootGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            RootGrid.Background = NormalBackBrush;
        }

        private void RootGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RootGrid.Background = MouseDownBackBrush;
        }

        private void RootGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RootGrid.Background = MouseEnterBackBrush;

            //之前没有点击，则设置为已经点击
            if (!IsActive)
            {
                SetNumberMessage(0);

                IsActive = true;
                //隐藏活跃标志
                ActiveTagRect.Visibility = Visibility.Visible;

                //如果有代理事件，则执行代理
                if (ButtonClick != null)
                {
                    ButtonClick.Invoke(this, ButtonID);
                }
            }
        }

        /// <summary>
        /// 虚拟点击，会将按钮的背景直接设置为正常背景
        /// </summary>
        public void Active_Virtual_Click()
        {
            RootGrid.Background = NormalBackBrush;

            //之前没有点击，则设置为已经点击
            if (!IsActive)
            {
                SetNumberMessage(0);

                IsActive = true;
                //隐藏活跃标志
                ActiveTagRect.Visibility = Visibility.Visible;

                //如果有代理事件，则执行代理
                if (ButtonClick != null)
                {
                    ButtonClick.Invoke(this, ButtonID);
                }
            }
        }

        /// <summary>
        /// 设置为不活跃状态
        /// </summary>
        public void DisActive()
        {
            IsActive = false;
            //隐藏活跃标志
            ActiveTagRect.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// test success , 设置角标数量
        /// </summary>
        /// <param name="value"></param>
        private void SetNumberMessage(int value)
        {
            if(value == 0)
            {
                //点击进入了页面，则清除消息
                NumberMessageBadged.Badge = null;
                //消息个数置为零
                MessageCount = 0;
            }
            else if(value > 0)
            {
                //点击进入了页面，则清除消息
                NumberMessageBadged.Badge = value;
                //消息个数置为零
                MessageCount = value;
            }
            //参数无效，抛出错误
            else
            {
                throw new Exception("Message Number Value Can Not Be Negative");
            }
           
        }
    }
}
