using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MasterCode.Code.Tools;
using MasterCode.Code;
using Excel = Microsoft.Office.Interop.Excel;

namespace MasterCode.Code
{
    /// <summary>
    /// 一次一个器件采集的30次数据
    /// </summary>
    [Serializable]
    public class OneDeviceDatasModel
    {
        /// <summary>
        /// 30次光强
        /// </summary>
        public List<float> LightIntensity = new List<float>();
        /// <summary>
        /// 30次电压
        /// </summary>
        public List<float> Voltage = new List<float>();
        /// <summary>
        /// 30次功率因素
        /// </summary>
        public List<float> PowerFactor = new List<float>();
        /// <summary>
        /// 30次未定义数据
        /// </summary>
        public List<float> Somethings = new List<float>();

        /// <summary>
        /// 增加一次整体数据到模型中
        /// test success , 2018.1.6
        /// </summary>
        /// <param name="light">光照强度</param>
        /// <param name="voltage">电压</param>
        /// <param name="power">功率因素</param>
        /// <returns>如果已经无法再添加了(达到了30组数据),则返回true，可以继续添加则为false</returns>
        public bool AddOnceData(float light , float voltage , float power , float something)
        {
            if(LightIntensity.Count >= 30)
            {
                return true;
            }
            else
            {
                LightIntensity.Add(light);
                Voltage.Add(voltage);
                PowerFactor.Add(power);
                Somethings.Add(something);

                if (LightIntensity.Count >= 30)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 将这个对象存储到xml文件中
        /// test success , 2018.1.6
        /// </summary>
        /// <returns>保存的文件名</returns>
        public String SaveAsXMLDatas()
        {
            String fileName;

            fileName = StringDispose.CreateRandomMD5() + ".xml";

            // Save object to a file named CarData.xml in XML format.
            XmlSerializer xmlFormat = new XmlSerializer(typeof(OneDeviceDatasModel));
            using (Stream fStream = new FileStream(PathStaicCollection.DatasDirPath + "\\" + fileName , FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, this);
            }

            return fileName;
        }

        /// <summary>
        /// 从一个xml文件中产生对象
        /// </summary>
        /// <param name="fileName">文件名字，不需要文件夹路径</param>
        /// <returns>产生的对象</returns>
        public static OneDeviceDatasModel CreateModelFromXMLFile(String fileName)
        {
            OneDeviceDatasModel result = null;

            XmlSerializer xmlFormat = new XmlSerializer(typeof(OneDeviceDatasModel));
            using (Stream fStream = new FileStream(PathStaicCollection.DatasDirPath + "\\" + fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                result = (OneDeviceDatasModel)xmlFormat.Deserialize(fStream);
            }

            return result;
        }
    }

    public class ExcelDatasModel
    {
        /// <summary>
        /// 所有器件的数据，Key代表器件的ID，Value代表着器件一次测量的30次数据
        /// Key 1 - 20,代表20组器件
        /// </summary>
        public Dictionary<int, OneDeviceDatasModel> AllDevicesDatas = new Dictionary<int, OneDeviceDatasModel>();


        public void SaveAsXLS()
        {
            //String 
        }

    }
}