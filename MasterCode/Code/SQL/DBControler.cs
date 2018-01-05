using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;
using MasterCode.Code;
using MasterCode.Code.Tools;
using MasterCode.MControls;

namespace MasterCode.Code.SQL
{
    /// <summary>
    /// 数据库控制器
    /// </summary>
    public class RecordDBControler
    {
        public static RecordDBControler UnityIns = null;
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "Record";
        /// <summary>
        /// 检查创建字符串
        /// </summary>
        private static String CreateRecordTableSqlStatement=
			"CREATE TABLE IF NOT EXISTS Record"
			+ "("
			+ "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
            + "DeviceID INTEGER NOT NULL,"
            + "FileName VARCHAR(50) NOT NULL,"
            + "Time VARCHAR(30) NOT NULL"
			+ ");" ;

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static String ConnectingString =
            "Data Source =" + PathStaicCollection.RecordDBPath + ";" + "Version=3";


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

        /// <summary>
        /// 增加一条记录到数据库
        /// test success , 2017.12.25
        /// </summary>
        /// <param name="model"></param>
        /// <param name="setNow">是否将记录时间修改到现在</param>
        /// <returns></returns>
        public bool AddARecord(RecordModel model, bool setNow)
        {
            bool isSucceeded = false;
            StringBuilder builder = new StringBuilder();

            if (setNow)
            {
                //将时间设置为现在的时间，也就是保存到数据库的时间
                model.SetTimeToNow();
            }

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                builder.Append("INSERT INTO ");
                builder.Append(TableName);
                builder.Append("(DeviceID,FileName,Time)");
                builder.Append(" ");

                builder.Append("VALUES");
                builder.Append("(");
                builder.Append( model.DeviceID + ",");
                builder.Append("\'" + model.FileName + "\',");
                builder.Append("\'" + model.TimeString + "\'");
                builder.Append(");");

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = builder.ToString();

                //如果成功插入则代表成功
                if (command.ExecuteNonQuery() == 1)
                {
                    isSucceeded = true;

                    String getIdCommandString = "SELECT last_insert_rowid() FROM " + TableName;

                    SQLiteCommand getIdCommand = sqlConnect.CreateCommand();
                    getIdCommand.CommandText = getIdCommandString;

                    //读取刚刚插入的ID，赋值给模型
                    using (SQLiteDataReader reader = getIdCommand.ExecuteReader())
                    {
                        reader.Read();
                        model.ID = reader.GetInt32(0);
                    }
                }
            }

            return isSucceeded;
        }

        /// <summary>
        /// 读取所有的记录
        /// test success
        /// </summary>
        /// <param name="isInOrder">是否将其降序排列，时间最晚的排在最前面</param>
        /// <returns></returns>
        public List<RecordModel> ReadAllRecord(bool isInOrder)
        {
            List<RecordModel> models = new List<RecordModel>();

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                String commandString ="SELECT * FROM " + TableName;
                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = commandString;

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        models.Add(new RecordModel(int.Parse(reader["ID"].ToString()),  reader["FileName"].ToString(), int.Parse(reader["DeviceID"].ToString()), reader["Time"].ToString()) );
                    }
                }
            }

            //是否要顺序排列输出
            if(isInOrder)
            {
                models.Sort((x, y) => -x.Time.CompareTo(y.Time));
            }

            return models;
        }

        /// <summary>
        /// 读取所有的记录
        /// test success
        /// </summary>
        /// <param name="isInOrder">是否将其降序排列，时间最晚的排在最前面</param>
        /// <param name="device">器件ID</param>
        /// <returns></returns>
        public List<RecordModel> ReadSpecialDeviceRecord(bool isInOrder , int device)
        {
            List<RecordModel> models = new List<RecordModel>();

            //连接数据库
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                String commandString = "SELECT * FROM " + TableName + " WHERE DeviceID=" + device;
                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = commandString;

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        models.Add(new RecordModel(int.Parse(reader["ID"].ToString()), reader["FileName"].ToString(), int.Parse(reader["DeviceID"].ToString()), reader["Time"].ToString()));
                    }
                }
            }

            //是否要顺序排列输出
            if (isInOrder)
            {
                models.Sort((x, y) => -x.Time.CompareTo(y.Time));
            }

            return models;
        }
    }
}
