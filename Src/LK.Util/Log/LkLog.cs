using System;
using System.Diagnostics;
using System.Reflection;

namespace LK.Util
{
    /// <summary>
    /// 可以透過LogParams設定相關參數
    /// </summary>
    public static class LkLog
    {

        private static LkLogParams _logParams;
        public static LkLogParams LogParams
        {
            get
            {
                if(_logParams == null)
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


        private static readonly object ms_lockObject = new object();
        private volatile static LogProvide _logProvide;
        private static LogProvide ms_logProvide
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
                        _logProvide = logFactory.CreatorProvide();

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
        private static void CreatorFactory(LogType logType)
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
                ms_logProvide = logFactory.CreatorProvide();
            }
        }

        /// <summary>
        /// 寫Log
        /// </summary>
        /// <param name="LogLevel">寫Log的等級</param>
        /// <param name="function">要記錄Log的函式名稱</param>
        /// <param name="description">要記錄Log的說明</param>
        public static void WriteLog(LogLevel logLevel, string function, string description)
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
        public static void WriteLog(LogLevel logLevel, string function, string description, Exception e)
        {
            ms_logProvide.WriteLog(logLevel, function, description, e);
        }

        /// <summary>
        /// 寫Log（一直持續寫的log不要用此函式）
        /// </summary>
        /// <param name="logLevel">寫Log的等級</param>
        /// <param name="description">要記錄Log的說明</param>
        public static void WriteLog(LogLevel logLevel, string description)
        {
            string function = GetFunctionName();
            ms_logProvide.WriteLog(logLevel, function, description);
        }

        /// <summary>
        /// 寫Log（一直持續寫的log不要用此函式）
        /// </summary>
        /// <param name="logLevel">寫Log的等級</param>
        /// <param name="e">Exception</param>
        public static void WriteLog(LogLevel logLevel, Exception e)
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
        public static void WriteLog(LogLevel logLevel, string description, Exception e)
        {
            string function = GetFunctionName();
            ms_logProvide.WriteLog(logLevel, function, description, e);
        }

        private static string GetFunctionName()
        {
            MethodBase caller = (new StackTrace()).GetFrame(2).GetMethod();
            string function = caller.DeclaringType.FullName;
            function = function.Substring(function.LastIndexOf('.') + 1, function.Length - function.LastIndexOf('.') - 1)
                + "." + caller.Name;
            return function;
        }

    }
}