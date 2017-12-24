using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using MasterCode.Code;

namespace MasterCode.Code.SQL
{
    /// <summary>
    /// 数据库控制器
    /// </summary>
    public class RecordDBControler
    {
        public static RecordDBControler UnityIns = null;

        private static String CreateRecordTableSqlStatement=
			"CREATE TABLE IF NOT EXISTS Record"
			+ "("
			+ "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
			+ "FileName VARCHAR(50) NOT NULL,"
            + "Time VARCHAR(20) NOT NULL"
			+ ");" ;

        private static String ConnectingString =
            @"Data Source =" + PathStaicCollection.RecordDBPath + ";" + "Version=3";

        public RecordDBControler()
        {
            CheckAndInit();
            UnityIns = this;
        }

        private void CheckAndInit()
        {
            //检查数据文件夹是否存在
            DirectoryInfo datasDir = new DirectoryInfo(PathStaicCollection.DatasDirPath);
            //不存在则创建
            if(!datasDir.Exists)
            {
                datasDir.Create();
            }

            //检查数据库是否存在，如不存在，则直接创建并且初始化
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = CreateRecordTableSqlStatement;

                command.ExecuteNonQuery();
            }
        }
    }
}
