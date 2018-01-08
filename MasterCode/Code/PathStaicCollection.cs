using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterCode.Code
{
    public static class PathStaicCollection
    {
        /// <summary>
        /// EXE文件运行文件夹，附带分隔符
        /// </summary>
        public static string RootOfExePath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 数据文件夹路径 , 没有分隔符
        /// </summary>
        public static String DatasDirPath = RootOfExePath + "Datas";
        /// <summary>
        /// 数据记录数据表路径
        /// </summary>
        public static String RecordDBPath = DatasDirPath + "\\Records.s3db";
        /// <summary>
        /// 用户文件
        /// </summary>
        public static String UserPreferFile = RootOfExePath + "UserPrefer.xml";
        /// <summary>
        /// 默认EXCEL文件保存文件夹路径，没有分隔符
        /// </summary>
        public static String DefaultExcelDir = RootOfExePath + "ExcelDatas";
    }
}
