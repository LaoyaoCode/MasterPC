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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire.Charts;
using System.Threading;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MasterCode.MControls;

namespace MasterCode
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window 
    {
        //现在正在显示的页面
        private UserControl NowDisplayPage = null;
        //现在正在活跃的按钮
        private MControls.ImageTextButton NowActiveButton = null;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化按钮点击事件并且赋予按钮ID
            //MainPageButton ID = 1
            MainPageControlMenuButotn.SetIDAndClick(this.MenuButtonClicked, 1);
            //DatasDisplayMenuButton ID = 2
            DatasDisplayMenuButton.SetIDAndClick(this.MenuButtonClicked, 2);
            //ComsControlMenuButton = 3
            ComsControlMenuButton.SetIDAndClick(this.MenuButtonClicked, 3);
            //ConsoleMenuButton = 4
            ConsoleMenuButton.SetIDAndClick(this.MenuButtonClicked, 4);
            //SettingMenuButton = 5
            SettingMenuButton.SetIDAndClick(this.MenuButtonClicked, 5);

            //模拟被点击的情况  ，初始显示主页面
            MainPageControlMenuButotn.Active_Virtual_Click();
        }

        //关闭程序按钮
        private void CloseButton_Click()
        {
            Application.Current.Shutdown();
        }

        //最小化按钮
        private void MinButton_Click()
        {
            this.WindowState = WindowState.Minimized;
        }

        //最顶级矩形条拖动
        private void TopBackRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //move the windows
            this.DragMove();
        }

        private void window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Normal)
            {
                BeginStoryboard((Storyboard)this.Resources["MaxAnimation"]);
            }
        }

        private void DisplayPage(UserControl display)
        {
            //移除原来所属父亲关系
            RootGrid.Children.Remove(display);
            if (NowDisplayPage != null)
            {
                MainDisplayArea.Content = null;
                NowDisplayPage.Visibility = Visibility.Collapsed;
                //恢复原来所属父亲关系
                RootGrid.Children.Add(NowDisplayPage);
            }

            MainDisplayArea.Content = display;
            display.Visibility = Visibility.Visible;

            NowDisplayPage = display ;
        }

        private void MenuButtonClicked(MControls.ImageTextButton button , int id)
        {
            //主页面按钮被点击
            if(id == 1)
            {
                DisplayPage(MainPageIns);

            }
            //数据显示页面按钮被点击
            else if (id == 2)
            {
                DisplayPage(DatasDisplayPageIns);
            }
            //串口控制页面按钮被点击
            else if (id == 3)
            {
                DisplayPage(ComsControlPageIns);
            }
            //控制台页面按钮被点击
            else if (id == 4)
            {
                DisplayPage(ConsolePageIns);
            }
            //设置页面被点击
            else if(id == 5)
            {
                DisplayPage(SettingPageIns);
            }



            //将之前活跃的按钮设置为不活跃
            if (NowActiveButton != null)
            {
                NowActiveButton.DisActive();
            }

            NowActiveButton = button;
        }

    }
}
