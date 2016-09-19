using System;
using System.IO;

namespace LK.Util
{
    internal class DeltaFileLog : CommonFileLog
    {
        protected override void DeleteOverdueLog()
        {
            DirectoryInfo parentDir = new DirectoryInfo(m_filePath);
            FileInfo[] files = parentDir.GetFiles();
            foreach (FileInfo file in files)
            {
                if(DateTime.Now.Subtract( file.LastWriteTime).Days >= LkLog.LogParams.LogRetentionDayCount)
                {
                    file.Delete();
                }
            }
        }

        protected override string GetFileName(LogLevel logLevel, string function)
        {
            string fileName = string.Empty;
            if ((int)logLevel > 2)
            {
                fileName = Path.Combine(m_filePath, "info_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            }
            else
            {
                fileName = Path.Combine(m_filePath, "faillog_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            }
            return fileName;
        }

        protected override string GetPrefix(LogLevel logLevel, string function)
        {
            return DateTime.Now.ToString("HH:mm:ss") + "[" + logLevel.ToString().PadRight(10) + "]" + "[" + function.PadRight(30) + "]";
        }
    }
}