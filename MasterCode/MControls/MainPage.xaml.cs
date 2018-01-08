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
        public MainPage()
        {
            InitializeComponent();
            
        }

        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CheckExcelDefaultDir();

            if (UserPerferControler.UnityIns.GetExcelPath() == "NULL")
            {
                ExcelPathTBlock.Text = "Excel Datas Dir : " + PathStaicCollection.DefaultExcelDir;
            }
            else
            {
                ExcelPathTBlock.Text = "Excel Datas Dir : " + UserPerferControler.UnityIns.GetExcelPath();
            }
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

        private void CheckExcelDefaultDir()
        {
            //默认数据文件夹是否存在
            DirectoryInfo datasDir = new DirectoryInfo(PathStaicCollection.DefaultExcelDir);
            //不存在则创建
            if (!datasDir.Exists)
            {
                datasDir.Create();
            }
        }

    }
}
