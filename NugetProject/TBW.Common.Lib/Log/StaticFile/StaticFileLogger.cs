using System;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

namespace TBW.Common.Lib.Log.StaticFile
{
    /// <summary>
    /// 写日志类
    /// </summary>
    public class FileLogger
    {
        #region 字段

        public static object _lock = new object();

        public static int day = 30;

        #endregion

        #region 写文件

        /// <summary>
        /// 写文件
        /// </summary>
        public static void WriteFile(string log, string path)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(delegate (object obj)
            {
                lock (_lock)
                {
                    if (!File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            
                        }
                    }

                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            #region 日志内容

                            string value = string.Format(@"{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                obj.ToString());

                            #endregion

                            sw.WriteLine(value);
                            sw.Flush();
                        }
                    }
                }
            }));
            thread.IsBackground = true;
            thread.Start(log);
        }

        #endregion

        #region 写日志

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string log)
        {
            CreLogDir(log, 1);
        }

        #endregion

        #region 定时删除文件记录

        /// <summary>
        /// 写错误日志
        /// </summary>
        public static void DeleteLogFile()
        {
            if (day <= 1)
            {
                day = 2;  //必须满足保留两个月的日志文件
            }
            string logpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DateTime dt = DateTime.Now.AddDays(-day);
            DeleteFile(logpath, dt);
        }



        /// <summary>
        /// 删除文件夹下的文件
        /// </summary>
        /// <param name="dirname">目录</param> 
        /// <param name="time">时间超过多少毫秒的删除</param>
        public static void DeleteFile(string dirname, DateTime time)
        {
            System.IO.DirectoryInfo dir = new DirectoryInfo(dirname);


            System.IO.DirectoryInfo[] files;
            files = dir.GetDirectories();

            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {

                    DateTime ts = files[i].LastWriteTime;
                    if (ts <= time)
                    {
                        files[i].Delete(true);
                    }
                }
            }
        }
        #endregion


        #region 写错误日志

        /// <summary>
        /// 写错误日志
        /// </summary>
        public static void WriteErrorLog(string log)
        {
            CreLogDir(log, 0);
        }

        #endregion

        #region 创建日志文件夹

        /// <summary>
        /// 创建日志文件夹
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="logtype">1上传日志 2错误日志</param>
        public static void CreLogDir(string log, int logtype)
        {

            string logpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //System.Windows.Forms.Application.StartupPath + "\\Logs";
            string MonthDirName = logpath + "\\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString();
            string logfilename = string.Empty;

            if (!Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }

            if (!Directory.Exists(MonthDirName))
            {
                Directory.CreateDirectory(MonthDirName);
            }

            if (logtype == 1)
            {
                logfilename = MonthDirName + "\\Log" + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
            }
            else
            {
                logfilename = MonthDirName + "\\ErrorLog" + DateTime.Now.Date.ToString("yyyyMMdd") + ".log";
            }
            WriteFile(log, logfilename);
        }

        #endregion
        /// <summary>
        /// 取得网站根目录的物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "log";
        }

    }
}
