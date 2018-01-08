using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterCode.Code;
using System.Xml;

namespace MasterCode.Code.Tools
{
    public class UserPerferControler
    {
        private const String RootNodeName = "User";
        private const String ExcelPathNodeName = "ExcelDicPath";
        public static UserPerferControler UnityIns = null;
        private String ExcelPath_In;

        public UserPerferControler()
        {
            UnityIns = this;

            ReadAllData();
        }

        private void ReadAllData()
        {
            XmlDocument document = new XmlDocument();
            document.Load(PathStaicCollection.UserPreferFile);

            XmlNode rootNode = document.SelectSingleNode(RootNodeName);

            ExcelPath_In = rootNode.SelectSingleNode(ExcelPathNodeName).InnerText;
        }

        /// <summary>
        /// 获取EXCEL文件保存路径
        /// 如果为默认没有被设置，则为NULL
        /// </summary>
        /// <returns></returns>
        public String GetExcelPath()
        {
            return ExcelPath_In;
        }

        /// <summary>
        /// 设置EXCEL文件保存路径
        /// </summary>
        /// <param name="path"></param>
        public void SetExcelPath(String path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(PathStaicCollection.UserPreferFile);

            XmlNode rootNode = document.SelectSingleNode(RootNodeName);

            rootNode.SelectSingleNode(ExcelPathNodeName).InnerText = path;
            document.Save(PathStaicCollection.UserPreferFile);

            ExcelPath_In = path;
        }
    }
}
