using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class Log
    {
        public static void WriteLog(Exception ex)
        {
            //如果是同一天的话，则打开文件在末尾写入。
            //如果不是同一天，则创建文件写入文件。
            //判断文件是否存在
            if (File.Exists(DateTime.Today.ToString("yyyyMMdd") + ".log"))
            {
                //如果文件存在，则向文件添加日志
                StreamWriter sw = new StreamWriter(DateTime.Today.ToString("yyyyMMdd") + ".log", true);
                sw.WriteLine("======================================");
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine(ex.Message);
                sw.WriteLine(ex.StackTrace);
                sw.Close();
                return;
            }

            //如果文件不存在，则创建文件后向文件添加日志
            StreamWriter sw2 = new StreamWriter(DateTime.Today.ToString("yyyyMMdd") + ".log", true);
            sw2.WriteLine("======================================");
            sw2.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sw2.WriteLine(ex.Message);
            sw2.Close();
        }
    }
}
