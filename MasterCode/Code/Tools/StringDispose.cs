using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace MasterCode.Code.Tools
{
    public static class StringDispose
    {
        /// <summary>
        /// 使用现在的时间，精确到毫秒级产生一个随机的唯一的MD5数据
        /// test success , 2017.12.25
        /// </summary>
        /// <returns></returns>
        public static String CreateRandomMD5()
        {
            return UseMD5(DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() + DateTime.Now.Millisecond);
        }

        /// <summary>
        /// 产生MD5数据
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        private static string UseMD5(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            //将每个字节转为16进制
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}