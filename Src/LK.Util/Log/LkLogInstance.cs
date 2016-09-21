using System;
using System.Diagnostics;
using System.Reflection;

namespace LK.Util
{
    /// <summary>
    /// 可以透過LogParams設定相關參數
    /// </summary>
    public class LkLogInstance
    {
        public LkLogInstance(LkLogParams param)
        {
            LogParams = param;
        }
        private LkLogParams _logParams;
        public LkLogParams LogParams
        {
            get
            {
                if (_logParams == null)
                {
                    _logParams = new LkLogParams();
                }
                return _logParams;
            }
            set
            {
                _logParams = value;
            }
        }


        private readonly object ms_lockObject = new object();
        private volatile static LogProvide _logProvide;
        private LogProvide ms_logProvide
        {
            get
            {
                lock (ms_lockObject)
                {
                    if (_logProvide == null)
                    {

                        ILogFactory logFactory = null;
                        switch (LogParams.LogType)
                        {
                            case LogType.File:
                                logFactory = new FileLogFactory();
                                break;
                        }
                        _logProvide = logFactory.CreatorProvide(LogParams);

                    }
                }
                return _logProvide;
            }
            set
            {
                _logProvide = value;
            }
        }

        /// <summary>
        /// 不開放
        /// </summary>
        /// <param name="logType"></param>
        private void CreatorFactory(LogType logType)
        {
            lock (ms_logProvide)
            {
                ILogFactory logFactory = null;
                switch (logType)
                {
                    case LogType.File:
                        logFactory = new FileLogFactory();
                        break;
                }
                ms_logProvide = logFactory.CreatorProvide(LogParams);
            }
        }

        /// <summary>
        /// 寫Log
        /// </summary>
        /// <param name="LogLevel">寫Log的等級</param>
        /// <param name="function">要記錄Log的函式名稱</param>
        /// <param name="description">要記錄Log的說明</param>
        public void WriteLog(LogLevel logLevel, string function, string description)
        {
            ms_logProvide.WriteLog(logLevel, function, description);
        }



        /// <summary>
        /// 寫Log
        /// </summary>
        /// <param name="LogLevel">寫Log的等級</param>
        /// <param name="function">要記錄Log的函式名稱</param>
        /// <param name="description">要記錄Log的說明</param>
        /// <param name="e">Exception</param>
        public void WriteLog(LogLevel logLevel, string function, string description, Exception e)
        {
            ms_logProvide.WriteLog(logLevel, function, description, e);
        }

        /// <summary>
        /// 寫Log（一直持續寫的log不要用此函式）
        /// </summary>
        /// <param name="logLevel">寫Log的等級</param>
        /// <param name="description">要記錄Log的說明</param>
        public void WriteLog(LogLevel logLevel, string description)
        {
            string function = GetFunctionName();
            ms_logProvide.WriteLog(logLevel, function, description);
        }

        /// <summary>
        /// 寫Log（一直持續寫的log不要用此函式）
        /// </summary>
        /// <param name="logLevel">寫Log的等級</param>
        /// <param name="e">Exception</param>
        public void WriteLog(LogLevel logLevel, Exception e)
        {
            string function = GetFunctionName();
            ms_logProvide.WriteLog(logLevel, function, e);
        }

        /// <summary>
        /// 寫Log
        /// </summary>
        /// <param name="LogLevel">寫Log的等級</param>
        /// <param name="description">要記錄Log的說明</param>
        /// <param name="e">Exception</param>
        public void WriteLog(LogLevel logLevel, string description, Exception e)
        {
            string function = GetFunctionName();
            ms_logProvide.WriteLog(logLevel, function, description, e);
        }

        private string GetFunctionName()
        {
            MethodBase caller = (new StackTrace()).GetFrame(2).GetMethod();
            string function = caller.DeclaringType.FullName;
            function = function.Substring(function.LastIndexOf('.') + 1, function.Length - function.LastIndexOf('.') - 1)
                + "." + caller.Name;
            return function;
        }

    }
}