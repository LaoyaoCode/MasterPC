﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MasterCode.Code.SQL
{
    public class RecordModel
    {
        public int ID;
        public String FileName;
        /// <summary>
        /// 时间和日期
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// 时间和日期字符串，用于在数据库中保存
        /// "2017/12/24$20:43:01"
        /// 日期2017/12/24+$(分隔符)+时间20:43:01
        /// </summary>
        public String TimeString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="fileName">文件名</param>
        public RecordModel(int id, String fileName)
        {
            FileName = fileName;
            ID = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名</param>
        public RecordModel(String fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="fileName">文件名</param>
        /// <param name="timeString">时间字符串</param>
        public RecordModel(int id, String fileName  ,String timeString )
        {
            FileName = fileName;
            ID = id;

            TimeString = timeString;

            String[] s = timeString.Split('$');
            String[] date = s[0].Split('/');
            String[] time = s[1].Split(':');

            Time = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]),
                int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="timeString">时间字符串</param>
        public RecordModel(String fileName, String timeString)
        {
            FileName = fileName;

            TimeString = timeString;

            String[] s = timeString.Split('$');
            String[] date = s[0].Split('/');
            String[] time = s[1].Split(':');

            Time = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]),
                int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
        }

        /// <summary>
        /// 将时间设置到现在
        /// 设置TimeString和Time
        /// </summary>
        public void SetTimeToNow()
        {
            Time = DateTime.Now;
            TimeString = DateTime.Now.ToShortDateString() + "$" +  DateTime.Now.ToLongTimeString();
        }
    }

}