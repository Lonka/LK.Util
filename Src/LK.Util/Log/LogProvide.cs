using System;

namespace LK.Util
{
    internal abstract class LogProvide
    {
        /// <summary>
        /// 最後一筆的fail Log msg
        /// </summary>
        protected string m_failLastMessage = string.Empty;

        /// <summary>
        /// 最後一筆的info Log msg
        /// </summary>
        protected string m_infoLastMessage = string.Empty;

        /// <summary>
        /// Log的呈現等級
        /// </summary>
        protected LogLevel m_limitLogLevel
        {
            get
            {
                return LkLog.LogParams.LogLevel;
            }
        }

        /// <summary>
        /// Log的字數最大值
        /// </summary>
        protected int m_maxLogSize
        {
            get
            {
                return LkLog.LogParams.MaxLogSize;
            }
        }

        public abstract void WriteLog(LogLevel logLevel, string function, string description);

        public abstract void WriteLog(LogLevel logLevel, string function, Exception e);

        public abstract void WriteLog(LogLevel logLevel, string function, string description, Exception e);

        /// <summary>
        /// 確認使用者設定的Log呈現等級來決定是否要寫Log
        /// </summary>
        /// <param name="logLevel">寫Log的等級</param>
        /// <returns></returns>
        protected bool CheckLevel(LogLevel logLevel)
        {
            bool result = false;
            if ((int)logLevel <= (int)m_limitLogLevel)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 取得Log訊息的實際內容
        /// </summary>
        /// <param name="logLevel">寫Log的等級</param>
        /// <param name="function">Function Name</param>
        /// <param name="msg">來源訊息</param>
        /// <returns></returns>
        protected virtual string GetLogMsg(LogLevel logLevel, string function, string msg)
        {
            string result = msg;
            if (m_maxLogSize > 0 && msg.Length > m_maxLogSize)
            {
                result = msg.Substring(0, m_maxLogSize, true);
            }
            if ((int)logLevel > 2)
            {
                lock (m_infoLastMessage)
                {
                    if (result.Length == m_infoLastMessage.Length && result.Equals(m_infoLastMessage))
                    {
                        result = "same...";
                    }
                    else
                    {
                        m_infoLastMessage = result;
                    }
                }
            }
            else
            {
                lock (m_failLastMessage)
                {
                    if (result.Length == m_failLastMessage.Length && result.Equals(m_failLastMessage))
                    {
                        result = "same...";
                    }
                    else
                    {
                        m_failLastMessage = result;
                    }
                }
            }
            return GetPrefix(logLevel, function) + result;
        }

        /// <summary>
        /// 取得Log訊息的前置詞
        /// </summary>
        /// <param name="logLevel">寫Log的等級</param>
        /// <param name="function">Function Name</param>
        /// <returns></returns>
        protected virtual string GetPrefix(LogLevel logLevel, string function)
        {
            return string.Format("[{0}]-[{1}]", DateTime.Now.ToString("HH:mm:ss"), function.PadRight(30));
        }
    }
}