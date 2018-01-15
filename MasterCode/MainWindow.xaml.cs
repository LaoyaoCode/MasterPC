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
using MasterCode.Code.SQL;
using MasterCode.Code;
using MasterCode.Code.Tools;

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
            //创建并且初始化Record DB控制器
            new RecordDBControler();
            //创建并且初始化用户喜好控制器
            new UserPerferControler();
            //创建并且初始化串口控制器
            new ComControler();

            //初始化按钮点击事件并且赋予按钮ID
            //MainPageButton ID = 1
            MainPageControlMenuButotn.SetIDAndClick(this.MenuButtonClicked, "1");
            //DatasDisplayMenuButton ID = 2
            DatasDisplayMenuButton.SetIDAndClick(this.MenuButtonClicked, "2");
            //ComsControlMenuButton = 3
            ComsControlMenuButton.SetIDAndClick(this.MenuButtonClicked, "3");
            //ConsoleMenuButton = 4
            ConsoleMenuButton.SetIDAndClick(this.MenuButtonClicked, "4");

            //模拟被点击的情况  ，初始显示主页面
            MainPageControlMenuButotn.Active_Virtual_Click();

            //增加控制台界面增加信息代理
            ConsolePageIns.ConsoleAddAMessage += ConsolePageIns_ConsoleAddAMessage;


            //test-------------------------------------
            Test();
        }

        //控制台页面增加了一条信息
        private void ConsolePageIns_ConsoleAddAMessage()
        {
            //没有正在显示控制台界面则增加气泡数目
            if(NowActiveButton != ConsoleMenuButton)
            {
                ConsoleMenuButton.AddNumber();
            }
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

        private void MenuButtonClicked(MControls.ImageTextButton button , String id)
        {
            //主页面按钮被点击
            if(id == "1")
            {
                DisplayPage(MainPageIns);

            }
            //数据显示页面按钮被点击
            else if (id == "2")
            {
                DisplayPage(DatasDisplayPageIns);
            }
            //串口控制页面按钮被点击
            else if (id == "3")
            {
                DisplayPage(ComsControlPageIns);
            }
            //控制台页面按钮被点击
            else if (id == "4")
            {
                DisplayPage(ConsolePageIns);
            }


            //将之前活跃的按钮设置为不活跃
            if (NowActiveButton != null)
            {
                NowActiveButton.DisActive();
            }

            NowActiveButton = button;
        }

        //test---------------------------------------------
        private void Test()
        {
            Random random = new Random();
            ExcelDatasModel model = new ExcelDatasModel();

            for(int counter = 1; counter <= 20; counter++)
            {
                for(int  i = 0; i < 30; i++)
                {
                    model.AllDevicesDatas[counter].AddOnceData((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                }
            }

            model.SaveAsXLSX();
        }
    }
}
