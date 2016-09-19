using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util
{
    public class LkLogParams
    {
        private  LogType _logType = LogType.File;

        /// <summary>
        /// 記錄寫Log的種類
        /// <para>預設：File</para>
        /// <para>對應：（Setting.ini）Setting -> LogType</para>
        /// </summary>
        public  LogType LogType
        {
            get
            {
                return _logType;
            }
            set
            {
                _logType = value;
            }
        }

        private string _logFile = AppDomain.CurrentDomain.BaseDirectory + "LogFiles";
        /// <summary>
        /// 記錄Log的資料夾路徑
        /// <para>預設：BaseDirectory\\LogFiles</para>
        /// <para>對應：（Setting.ini）Path Setting -> LogFile</para>
        /// </summary>
        public string LogFile
        {
            get
            {
                return _logFile;
            }
            set
            {
                _logFile = value;
            }
        }


        private LogLevel _logLevel = LogLevel.Error;
        /// <summary>
        /// 寫Log的等級，越嚴重數字越小，所以只會寫小於設定等級的Log
        /// <para>預設：Error</para>
        /// <para>對應：（Setting.ini）Setting -> LogLevel</para>
        /// </summary>
        public LogLevel LogLevel
        {
            get
            {
                return _logLevel;
            }
            set
            {
                _logLevel = value;
            }
        }


        private int _maxLogSize = -1;
        /// <summary>
        /// 寫Log的最大字數限制
        /// <para>預設：-1</para>
        /// <para>對應：（Setting.ini）Setting -> MaxLogSize</para>
        /// </summary>
        public int MaxLogSize
        {
            get
            {
                return _maxLogSize;
            }
            set
            {
                _maxLogSize = value;
            }
        }

        private LogImplType _logImplType = LogImplType.FileCommon;
        /// <summary>
        /// 記錄寫Log factory的種類
        /// <para>預設：-1</para>
        /// <para>對應：（Setting.ini）Setting -> LogImplType</para>
        /// </summary>
        public LogImplType LogImplType
        {
            get
            {
                return _logImplType;
            }
            set
            {
                _logImplType = value;
            }
        }


        private int _logRetentionDayCount = -1;
        /// <summary>
        /// Log保留的天數
        /// <para>預設：-1</para>
        /// <para>對應：（Setting.ini）Setting -> LogRetentionDayCount</para>
        /// </summary>
        public int LogRetentionDayCount
        {
            get
            {
                return _logRetentionDayCount;
            }
            set
            {
                _logRetentionDayCount = value;
            }
        }
    }
}
