using System;
using System.Collections.Generic;
using System.Text;
using LK.Util;
using System.IO;

namespace LK.Util
{
    internal class CommonFileLog : LogProvide
    {
        /// <summary>
        /// 存Log的資料夾路徑
        /// </summary>
        protected string m_filePath
        {
            get
            {
                return LkLog.LogParams.LogFile;
            }
        }

        protected virtual string GetFileName(LogLevel logLevel, string function)
        {
            return Path.Combine(Path.Combine(m_filePath, DateTime.Now.ToString("yyyy-MM-dd")), logLevel.ToString() + ".log");
        }

        protected virtual void DoWriteLog(LogLevel logLevel, string function, string msg)
        {
            DeleteOverdueLog();
            if (CheckLevel(logLevel))
            {
                DoWriteLog(GetFileName(logLevel,function)
                    , GetLogMsg(logLevel, function, msg));
            }
        }

        protected virtual void DeleteOverdueLog()
        {
            if (LkLog.LogParams.LogRetentionDayCount <= 0)
                return;
            DirectoryInfo parentDir = new DirectoryInfo(m_filePath);
            DirectoryInfo[] dirs = parentDir.GetDirectories();
            DateTime dt = new DateTime();
            foreach (DirectoryInfo dir in dirs)
            {
                if (DateTime.TryParse(dir.Name, out dt) && DateTime.Now.Subtract(dt).Days >= LkLog.LogParams.LogRetentionDayCount)
                {
                    dir.Delete(true);
                }
            }
        }

        private readonly object m_writeLogLockObj = new object();

        protected virtual void DoWriteLog(string fileName, string msg)
        {
            bool retry = false;
            lock (m_writeLogLockObj)
            {
                StreamWriter writer = null;
                try
                {
                    writer = File.AppendText(fileName);
                    writer.WriteLine(msg);
                }
                catch (Exception e)
                {
                    if (e is DirectoryNotFoundException)
                    {
                        retry = true;
                    }
                }
                if (writer != null)
                {
                    try
                    {
                        writer.Close();
                    }
                    catch { }
                    writer = null;
                }
                if (retry) // Create Directory
                {
                    writer = null;
                    try
                    {
                        Directory.CreateDirectory(fileName.Replace(Path.GetFileName(fileName), string.Empty));
                        writer = File.AppendText(fileName);
                        writer.WriteLine(msg);
                    }
                    catch { }
                    if (writer != null)
                    {
                        try
                        {
                            writer.Close();
                        }
                        catch { }
                        writer = null;
                    }
                }
            }
        }

        public override void WriteLog(LogLevel logLevel, string function, string description)
        {
            DoWriteLog(logLevel, function, description);
        }

        public override void WriteLog(LogLevel logLevel, string function, Exception e)
        {
            DoWriteLog(logLevel, function, e.Message + e.Source + e.StackTrace);
        }

        public override void WriteLog(LogLevel logLevel, string function, string description, Exception e)
        {
            DoWriteLog(logLevel, function, description + "," + e.Message + e.Source + e.StackTrace);
        }
    }
}
