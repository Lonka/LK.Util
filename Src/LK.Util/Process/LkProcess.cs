using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LK.Util
{
    /// <summary>
    /// Process operation(Static)
    /// </summary>
    public static class LkProcess
    {
        // http://jeremy.tfeng.org/?p=567
        // http://www.dotblogs.com.tw/chou/archive/2009/04/26/8180.aspx
        [DllImport("user32", EntryPoint = "FindWindowA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private static TimeSpan Tick = new TimeSpan(0, 0, 0, 0, 50 /* milliseconds */);
        public const int WM_CLOSE = 0x10;

        public static void Run(string fileName)
        {
            ProcessStartInfo pi = new ProcessStartInfo(fileName);
            System.Diagnostics.Process.Start(pi);
        }

        /// <summary>
        /// run specified program with arguments
        /// </summary>
        public static void Run(string fileName, string arguments)
        {
            ProcessStartInfo pi = new ProcessStartInfo(fileName, arguments);
            System.Diagnostics.Process.Start(pi);
        }

        public static void Close(string lpClassName, string lpWindowName)
        {
            if (Exists(lpClassName, lpWindowName))
            {
                PostMessage(FindWindow(lpClassName, lpWindowName), WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }
        public static void WaitClose(string lpClassName, string lpWindowName, TimeSpan timeout)
        {
            CloseThread closeThread = new CloseThread(lpClassName, lpWindowName, timeout);
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart (closeThread.Close));
            thread.Start();
        }

        /// <summary>
        /// Pauses execution until the requested window exists or timeout happen.
        /// </summary>
        /// <returns>
        /// Returns false if timeout occurred.
        /// </returns>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <param name="timeout">Timeout in seconds</param>
        public static bool WaitExists(string lpClassName, string lpWindowName, TimeSpan timeout)
        {
            if (!Exists(lpClassName, lpWindowName))
            {
                if (timeout <= TimeSpan.Zero)
                    return false;

                System.Threading.Thread.Sleep(Tick);

                timeout -= Tick;

                return WaitExists(lpClassName, lpWindowName, timeout);
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Pauses execution until the requested window exist.
        /// </summary>
        public static void WaitExists(string lpClassName, string lpWindowName)
        {
            if (!Exists(lpClassName, lpWindowName))
            {
                System.Threading.Thread.Sleep(Tick);
                WaitExists(lpClassName, lpWindowName);
            }
        }

        ///// <summary>
        ///// Pauses execution until the requested window does not exist.
        ///// </summary>
        //public static void WaitClose(string lpClassName, string lpWindowName)
        //{
        //    if (Exists(lpClassName, lpWindowName))
        //    {
        //        System.Threading.Thread.Sleep(Tick);
        //        WaitClose(lpClassName, lpWindowName);
        //    }
        //}
        ///// <summary>
        ///// Pauses execution until the requested window does not exist or timeout happen.
        ///// </summary>
        ///// <returns>
        ///// Returns false if timeout occurred.
        ///// </returns>
        ///// <param name="lpClassName"></param>
        ///// <param name="lpWindowName"></param>
        ///// <param name="timeout">Timeout in seconds</param>
        //public static bool WaitClose(string lpClassName, string lpWindowName, TimeSpan timeout)
        //{
        //    if (Exists(lpClassName, lpWindowName))
        //    {
        //        if (timeout <= TimeSpan.Zero)
        //            return false;

        //        System.Threading.Thread.Sleep(Tick);

        //        timeout -= Tick;

        //        return WaitClose(lpClassName, lpWindowName, timeout);
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}


        /// <summary>
        /// determine specified window exists
        /// </summary>
        public static bool Exists(string lpClassName, string lpWindowName)
        {
            IntPtr hwd = default(IntPtr);
            hwd = FindWindow(lpClassName, lpWindowName);
            //如果是hwd.Equals(IntPtr.Zero) = true即未啟動程式
            //如果是hwd.Equals(IntPtr.Zero) = false即已啟動程式
            //因為是回傳是否啟動了  故加!讓啟動為true
            return !hwd.Equals(IntPtr.Zero);
        }

    }
}
