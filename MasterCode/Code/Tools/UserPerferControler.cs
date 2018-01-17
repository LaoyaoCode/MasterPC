using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterCode.Code;
using System.Xml;
using System.IO.Ports;

namespace MasterCode.Code.Tools
{
    public class UserPerferControler
    {
        private const String RootNodeName = "User";
        private const String ExcelPathNodeName = "ExcelDicPath";
        private const String ParityNodeName = "Parity";
        private const String HandShakeNodeName = "HandShake";
        private const String BaudRateNodeName = "BaudRate";
        private const String StopBitsNodeName = "StopBits";
        private const String PeroidHoursNodeName = "PeroidHours";

        public static UserPerferControler UnityIns = null;
        private String ExcelPath_In;
        private ComControler.PortParaSetStruct PortPara = new ComControler.PortParaSetStruct();
        private int PeroidHours = 0;

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
            PortPara.BaudRate = int.Parse(rootNode.SelectSingleNode(BaudRateNodeName).InnerText);
            PortPara.ChooseHandShake =(Handshake) Enum.Parse(typeof(Handshake), rootNode.SelectSingleNode(HandShakeNodeName).InnerText);
            PortPara.ChooseParity = (Parity)Enum.Parse(typeof(Parity), rootNode.SelectSingleNode(ParityNodeName).InnerText);
            PortPara.ChooseStopBits = (StopBits)Enum.Parse(typeof(StopBits), rootNode.SelectSingleNode(StopBitsNodeName).InnerText);
            PeroidHours = int.Parse(rootNode.SelectSingleNode(PeroidHoursNodeName).InnerText);
        }

        public int GetPeroidHours()
        {
            return PeroidHours;
        }

        public void SetPeroidHours(int hour)
        {
            XmlDocument document = new XmlDocument();
            document.Load(PathStaicCollection.UserPreferFile);

            XmlNode rootNode = document.SelectSingleNode(RootNodeName);

            rootNode.SelectSingleNode(PeroidHoursNodeName).InnerText = hour.ToString();
            document.Save(PathStaicCollection.UserPreferFile);

            PeroidHours = hour;
        }

        public ComControler.PortParaSetStruct GetPortPara()
        {
            return PortPara;
        }
        /// <summary>
        /// 设置串口参数
        /// </summary>
        /// <param name="para"></param>
        public void SetPortPara(ComControler.PortParaSetStruct para)
        {
            XmlDocument document = new XmlDocument();
            document.Load(PathStaicCollection.UserPreferFile);

            XmlNode rootNode = document.SelectSingleNode(RootNodeName);

            rootNode.SelectSingleNode(BaudRateNodeName).InnerText = para.BaudRate.ToString();
            rootNode.SelectSingleNode(HandShakeNodeName).InnerText = para.ChooseHandShake.ToString();
            rootNode.SelectSingleNode(ParityNodeName).InnerText = para.ChooseParity.ToString();
            rootNode.SelectSingleNode(StopBitsNodeName).InnerText = para.ChooseStopBits.ToString();

            document.Save(PathStaicCollection.UserPreferFile);

            PortPara = para;
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
