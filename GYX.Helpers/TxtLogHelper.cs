using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

/// <summary>
/// 记录文本日志
/// </summary>
namespace GYX.Helpers
{
    /// <summary>
    /// 日志结果
    /// </summary>
    public class TxtLogResult
    {
        public static string success = "成功";
        public static string fail = "失败";
        public static string normal = "正常";
        public static string error = "出错";
    }
    public class TxtLogHelper
    {
        static string strPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log\\";

        /// <summary>
        /// 对外提供的日志接口，都可调用
        /// </summary>
        /// <param name="strMsg">日志信息</param>
        /// <param name="strResult">操作结果</param>
        public static void WriteLog(string strMsg, string strResult)
        {
            checkFile();
            string logName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            try
            {
                string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string strData = "\r\n" + strDateTime + "\t\t" + strResult + "\t\t" + strMsg;
                FileStream fs = new FileStream(strPath + logName, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine(strData);

                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            { }
            try
            {
                BakLog(strPath + logName);
            }
            catch { }
        }

        /// <summary>
        /// 检查记录日志需要的路径是否存在
        /// </summary>
        public static void checkFile()
        {
            string path = strPath;
            if (path.EndsWith("\\"))
                path = path.Substring(0, path.Length - 1);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 如果当前日志文件大于10M，备份日志后再新建日志文件
        /// </summary>
        /// <param name="strLogFilePath"></param>
        private static void BakLog(string strLogFilePath)
        {
            FileInfo file = new FileInfo(strLogFilePath);
            //如果大于10M，则进行备份
            if (file.Length > 1024 * 1024 * 10)
            {
                File.Move(strLogFilePath, strLogFilePath.Replace(".txt", "截止到" + DateTime.Now.ToString("HHmmss") + ".txt"));
                File.Create(strLogFilePath).Close();
            }
        }
    }
}
