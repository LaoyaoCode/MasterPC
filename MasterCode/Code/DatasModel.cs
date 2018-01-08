using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MasterCode.Code.Tools;
using MasterCode.Code;
using OfficeOpenXml;

namespace MasterCode.Code
{
    /// <summary>
    /// 一次一个器件采集的30次数据
    /// </summary>
    [Serializable]
    public class OneDeviceDatasModel
    {
        /// <summary>
        /// 保存的最大数据个数
        /// </summary>
        public const int MaxCount = 30;
        /// <summary>
        /// 30次光强
        /// 从下标0-29,0为最开始采集的数据，29为最后采集的数据
        /// </summary>
        public List<float> LightIntensity = new List<float>();
        /// <summary>
        /// 30次电压
        /// 从下标0-29,0为最开始采集的数据，29为最后采集的数据
        /// </summary>
        public List<float> Voltage = new List<float>();
        /// <summary>
        /// 30次功率因素
        /// 从下标0-29,0为最开始采集的数据，29为最后采集的数据
        /// </summary>
        public List<float> PowerFactor = new List<float>();
        /// <summary>
        /// 30次未定义数据
        /// 从下标0-29,0为最开始采集的数据，29为最后采集的数据
        /// </summary>
        public List<float> Somethings1 = new List<float>();
        /// <summary>
        /// 30次未定义数据
        /// 从下标0-29,0为最开始采集的数据，29为最后采集的数据
        /// </summary>
        public List<float> Somethings2 = new List<float>();

        /// <summary>
        /// 增加一次整体数据到模型中
        /// test success , 2018.1.6
        /// </summary>
        /// <param name="light">光照强度</param>
        /// <param name="voltage">电压</param>
        /// <param name="power">功率因素</param>
        /// <param name="something1">某些东西1</param>
        /// <param name="something2">某些东西2</param>
        /// <returns>如果已经无法再添加了(达到了30组数据),则返回true，可以继续添加则为false</returns>
        public bool AddOnceData(float light , float voltage , float power , float something1 , float something2)
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
                Somethings1.Add(something1);
                Somethings2.Add(something2);

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

        public float GetLightIntensityAverage()
        {
            float sum = 0.0f;

            for(int counter = 0; counter < LightIntensity.Count; counter++)
            {
                sum += LightIntensity[counter];
            }

            if(LightIntensity.Count == 0)
            {
                return 0.0f;
            }
            else
            {
                return sum / LightIntensity.Count;
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

        public int Count()
        {
            return LightIntensity.Count;
        }
    }

    public class ExcelDatasModel
    {
        /// <summary>
        /// 所有器件的数据，Key代表器件的ID，Value代表着器件一次测量的30次数据
        /// Key 1 - 20,代表20组器件
        /// </summary>
        public Dictionary<int, OneDeviceDatasModel> AllDevicesDatas = new Dictionary<int, OneDeviceDatasModel>();

        public ExcelDatasModel()
        {
            CheckExcelDefaultDir();

            for (int counter = 1; counter <= 20; counter ++)
            {
                AllDevicesDatas.Add(counter, new OneDeviceDatasModel());
            }
        }

        /// <summary>
        /// 将模型保存为XLSX数据，自动命名，但花费时间较长，故而不能在主线程中执行
        /// </summary>
        public void SaveAsXLSX()
        {
            String fileName;
            int counterForRow = 2;
            ExcelWorksheet workSheet;

            if (UserPerferControler.UnityIns.GetExcelPath() == "NULL")
            {
                fileName = PathStaicCollection.DefaultExcelDir + "\\" + DateTime.Now.ToLongDateString() + "__" + DateTime.Now.ToLongTimeString().Replace(':','.') + ".xlsx";
            }
            else
            {
                fileName = UserPerferControler.UnityIns.GetExcelPath() + "\\" + DateTime.Now.ToLongDateString() + "__" + DateTime.Now.ToLongTimeString().Replace(':', '.') + ".xlsx";
            }

            using (var p = new ExcelPackage())
            {
                workSheet = p.Workbook.Worksheets.Add("数据");

                // Establish column headings in cells.
                workSheet.Cells[1, 1].Value = "器件ID";
                workSheet.Cells[1, 2].Value = "采集编号(0为最先采集的数据)";
                workSheet.Cells[1, 3].Value = "光照强度";
                workSheet.Cells[1, 4].Value = "电压";
                workSheet.Cells[1, 5].Value = "功率因素";
                workSheet.Cells[1, 6].Value = "光照平均值";
                workSheet.Cells[1, 7].Value = "Somethings1";
                workSheet.Cells[1, 8].Value = "Somethings2";

                for (int counterForDevice = 1; counterForDevice <= 20; counterForDevice++)
                {
                    for (int counterForData = 0; counterForData < AllDevicesDatas[counterForDevice].Count(); counterForData++)
                    {
                        workSheet.Cells[counterForRow, 1].Value = counterForDevice.ToString();
                        workSheet.Cells[counterForRow, 2].Value = counterForData.ToString();
                        workSheet.Cells[counterForRow, 3].Value = AllDevicesDatas[counterForDevice].LightIntensity[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 4].Value = AllDevicesDatas[counterForDevice].Voltage[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 5].Value = AllDevicesDatas[counterForDevice].PowerFactor[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 6].Value = AllDevicesDatas[counterForDevice].GetLightIntensityAverage().ToString("0.00");
                        workSheet.Cells[counterForRow, 7].Value = AllDevicesDatas[counterForDevice].Somethings1[counterForData].ToString("0.00");
                        workSheet.Cells[counterForRow, 8].Value = AllDevicesDatas[counterForDevice].Somethings2[counterForData].ToString("0.00");

                        counterForRow++;
                    }
                }

                p.SaveAs(new FileInfo(fileName));
            }  
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